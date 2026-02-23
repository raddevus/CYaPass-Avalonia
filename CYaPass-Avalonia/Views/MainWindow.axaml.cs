using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;           // For Application
using Avalonia.Styling;   // For ThemeVariant
using Avalonia.Media;     // For Brushes
using Avalonia.Controls;
using Avalonia.Interactivity;
using TextCopy;
using NewLibre.Services;
using NewLibre.Models;
using CYaPass_Avalonia.ViewModels;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closing += async (s,e)  =>  { 
           await SaveConfig();
        };
        LoadAppConfig();
        InitThemeChangeHandler();
    }

    protected override void OnOpened(EventArgs e){
       base.OnOpened(e);
/*       var vm = (MainWindowViewModel)DataContext;

         vm.allSiteKeys.Add("abc");
         vm.allSiteKeys.Add("zippy");
         vm.allSiteKeys.Add("supersite"); */

        CheckThemeVariant(); 
    }

    private void CheckThemeVariant(){
       Console.WriteLine($"theme: {ActualThemeVariant}"); 
      if (ActualThemeVariant == ThemeVariant.Dark)
      {
          // Apply dark mode background color
          LeftSide.Background = Brushes.DarkBlue;
          RightSide.Background = Brushes.DarkGreen;
      }
      else
      {
          // Apply light mode background color
          LeftSide.Background = Brushes.LightBlue;
          RightSide.Background = Brushes.LightYellow;
      }
    }

    private void InitThemeChangeHandler(){
       if (Application.Current == null){return;}
          Application.Current.ActualThemeVariantChanged += (s, e) =>
         {
                Console.WriteLine($"ThemeVariant: {Application.Current.ActualThemeVariant}"); 
                CheckThemeVariant();
             if (Application.Current.ActualThemeVariant == ThemeVariant.Dark)
             {
             }
             else
             {
                 // Switched to light mode
             }
         };
   }

    async public void LoadAppConfig(){
      
      var cfile = AppConfig.ConfigFile;  
      if (File.Exists(cfile)){
         try{
            var configJson = await File.ReadAllTextAsync(cfile);
            Console.WriteLine($" got it!!!!! => {configJson}");
            var lc = JsonSerializer.Deserialize<AppConfig>(configJson);
            Console.WriteLine($"last key: {lc?.LastSelectedKey} transferUrl: {lc?.TransferUrl}"); 
            MultiHashCB?.IsChecked = lc?.MultiHashIsOn;
            SiteKeys.SelectedItem = lc?.LastSelectedKey;
            MultiHashUD?.Value = lc?.MultiHashCount;
         }
         catch (Exception ex){
            Console.WriteLine($"Coudn't read config file:{cfile} - {ex.Message}");
         }
      }
      else{
         Console.WriteLine($"Couldn't do the work, because test file doesn't exist: {cfile}");
      }
    }
    async private Task<bool> SaveConfig(){

      MainWindowViewModel? vm = DataContext as MainWindowViewModel;
      vm?.CyaConfig.MultiHashIsOn = MultiHashCB?.IsChecked ?? false;
      vm?.CyaConfig.MultiHashCount = (int)(MultiHashUD?.Value ?? 0);

      vm?.CyaConfig.LastSelectedKey = SiteKeys?.SelectedItem?.ToString();

     Console.WriteLine(AppConfig.ConfigFile);
   
     bool result = await vm?.CyaConfig?.Save();
     Console.WriteLine($"result: {result}");
     return true;
    }

    public void ClearHandler(object? sender, RoutedEventArgs e){
      Console.WriteLine("Clear the grid.");
      PwdGrid.Reset();
      HidePatternCheckBox.IsChecked = false;
    }
    
    public void SetHidePattern(object? sender, RoutedEventArgs e){
      Console.WriteLine("Setting Hide Pattern bool");
      PwdGrid.IsPatternHidden = (sender as CheckBox)?.IsChecked ?? false;
      System.Console.WriteLine($"isPatternHidden: {PwdGrid.IsPatternHidden}");
      PwdGrid.ForceRender();
    }

   private async void AddUppercaseChanged(object? sender, RoutedEventArgs e){
      
      PwdGrid.IsUppercase = (sender as CheckBox)?.IsChecked ?? false;
      PwdGrid.UpdatePassword();
   }

   private async void MultiHashCheckChanged(object? sender, RoutedEventArgs e){
      if (MultiHashCB.IsChecked ?? false){
         PwdGrid.MultiHash = (int)(MultiHashUD?.Value ?? 0);
      }
      else{
         PwdGrid.MultiHash = 0;
      }
      PwdGrid.UpdatePassword();
   }

   private async void MultiHashChanged(object? sender, RoutedEventArgs e){
      if (MultiHashCB?.IsChecked ?? false){
         PwdGrid.MultiHash = (int)(MultiHashUD.Value ?? 0);
         PwdGrid.UpdatePassword();
      }
   }

   private async void SpecialCharsChanged(object? sender, RoutedEventArgs e){
      PwdGrid.IsSpecialChars = SpecialCharsCB?.IsChecked ?? false;
      if (PwdGrid.IsSpecialChars){
        PwdGrid.SpecialChars = SpecialCharsTB.Text;
      }
      PwdGrid.UpdatePassword();
   }
   private async void MaxLengthChanged(object? sender, RoutedEventArgs e){
      PwdGrid.IsMaxLength = MaxLengthCB?.IsChecked ?? false;
      PwdGrid.MaxLength = (int)(MaxLengthUD.Value ?? 64);
      PwdGrid.UpdatePassword();
      Console.WriteLine($"maxlength: {MaxLengthUD.Value}");
   }

   private async void SiteKeyChanged(object? sender, RoutedEventArgs e){
      Console.WriteLine($"{sender}  {(e as SelectionChangedEventArgs)}");
      var vm = (MainWindowViewModel)DataContext;
       if (sender is ListBox lb && lb.SelectedItem is string text)
       {
          Console.WriteLine($"{vm.allSiteKeys.GetItemByKey(lb.SelectedItem.ToString()).MaxLength}");
           Console.WriteLine($"Selected text: {text}");
           PwdGrid.SiteKey = text;
           if (MultiHashCB.IsChecked ?? false){
              PwdGrid.MultiHash = (int)(MultiHashUD.Value ?? 0);
           }
           else{
              PwdGrid.MultiHash = 0;
           }
           PwdGrid.UpdatePassword();
       }
   }

   private async void PasswordTextChanged(object? sender, RoutedEventArgs e){
         try{
            ClipboardService.SetText(PwdTextBox?.Text ?? string.Empty);
         }
         catch (Exception ex){
            Console.WriteLine("Couldn't copy to clipboard.");
         }
   }

    async public void ImportSiteKeys(object? sender, RoutedEventArgs e){
      Console.WriteLine("Importing SiteKeys...");
      var cyasvc = new CyaService("demoKeys2022","https://newlibre.com/LibreStore/");
      var result = await cyasvc.GetCyaData();
      var encryptedSiteKeys = result.CyaBucket.Data;
      var iv = result.CyaBucket.Iv;
      Console.WriteLine($"{encryptedSiteKeys}");
      Crypton c = new();
      string decryptedData = string.Empty;
      var isSuccessDecrypt = c.Decrypt(encryptedSiteKeys, PwdTextBox.Text, iv, out decryptedData);
      if (isSuccessDecrypt){
         Console.WriteLine($"{decryptedData}"); 
         var vm = (MainWindowViewModel)DataContext;
         var downloadedSiteKeys = JsonSerializer.Deserialize<List<SiteKey>>(decryptedData);
         foreach (SiteKey s in downloadedSiteKeys){ vm.allSiteKeys.Add(s);} 
      }
      else{
         Console.WriteLine("The data couldn't be decrypted. You may have used an incorrect password key or the data may be corrupted.");
      }
    }

    private async void AddTestSiteKeys(object? sender, RoutedEventArgs e){
         var vm = (MainWindowViewModel)DataContext;
        vm.allSiteKeys.Add(new SiteKey{Key="test1"});
        vm.allSiteKeys.Add(new SiteKey{Key="test2"});
        vm.allSiteKeys.Add(new SiteKey{Key="test3"});
    }

   private async void DeleteSiteKey(object? sender, RoutedEventArgs e){
      if (SiteKeys.SelectedItem == null){return;}
      var vm = (MainWindowViewModel)DataContext;
     Console.WriteLine($"{SiteKeys.SelectedItem}");
     bool isDeleted = vm.allSiteKeys.Remove(new SiteKey{Key=SiteKeys.SelectedItem.ToString()});
     Console.WriteLine($"isDeleted: ${isDeleted}");
   }

   private async void OnAddSiteKeyClick(object? sender, RoutedEventArgs e){
      MainWindowViewModel vm = (MainWindowViewModel)DataContext;
      var msg = new SiteKeyMsgBox("Please type the SiteKey you'd like to add.");
       bool result =  await msg.ShowDialog<bool>(this);
       if (result)
       {
           // User clicked OK
            Console.WriteLine($"User selected OK: {msg.SiteKey}");
            if (!string.IsNullOrEmpty(msg.SiteKey.Key)){
               vm.allSiteKeys.Add(new SiteKey{Key = msg.SiteKey.Key});
            }
            SiteKeys.SelectedItem = msg.SiteKey;
            // initial value gets set
            if (!string.IsNullOrEmpty(SiteKeys.SelectedItem?.ToString())){
               PwdGrid.SiteKey = SiteKeys.SelectedItem?.ToString();
               if (MultiHashCB.IsChecked ?? false){
                  PwdGrid.MultiHash = (int)(MultiHashUD?.Value ?? 0);
                 }
                 else{
                    PwdGrid.MultiHash = 0;
                 }
               PwdGrid.UpdatePassword();
            }
       }
       else
       {
           // User clicked Cancel
           Console.WriteLine("User cancelled");
       }
    }

}
