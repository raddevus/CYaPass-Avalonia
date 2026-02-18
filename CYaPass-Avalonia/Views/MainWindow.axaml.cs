using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
        this.Closing += (s,e) => { 
           SaveConfig();
        };
        LoadAppConfig();
    }

    async public void LoadAppConfig(){
      
      var cfile = AppConfig.ConfigFile;  
      var vm = (MainWindowViewModel)DataContext;
      if (File.Exists(cfile)){
         try{
         var configJson = await File.ReadAllTextAsync(cfile);
         Console.WriteLine($" got it!!!!! => {configJson}");
         var lc = JsonSerializer.Deserialize<AppConfig>(configJson);
         Console.WriteLine($"last key: {lc.LastSelectedKey} transferUrl: {lc.TransferUrl}"); 
         MultiHashCB?.IsChecked = lc.MultiHashIsOn;
         MultiHashUD?.Value = lc.MultiHashCount;
         MultiHashCB.InvalidateVisual();
         MultiHashUD.InvalidateVisual();
         }
         catch (Exception ex){
            Console.WriteLine($"Coudn't read config file:{cfile} - {ex.Message}");
         }
      }
      else{
         Console.WriteLine($"Couldn't do the work, because test file doesn't exist: {cfile}");
      }
    }
    async private void SaveConfig(){

      var vm = (MainWindowViewModel)DataContext;
      Console.WriteLine($"vm : {vm == null}");
     Console.WriteLine("I'm closing.");
     Console.WriteLine($"CyaConfig : {vm.CyaConfig == null}");
     vm.CyaConfig?.MultiHashIsOn = MultiHashCB?.IsChecked ?? false;
     vm.CyaConfig?.MultiHashCount = (int)MultiHashUD?.Value;
     vm.CyaConfig?.LastSelectedKey = "Rad-Test";
     Console.WriteLine(AppConfig.ConfigFile);
     Console.WriteLine("going to save...");
     
     bool result = await vm.CyaConfig?.Save();
     System.Threading.Thread.Sleep(500);
     Console.WriteLine($"result: {result}");
    }

    public void ClearHandler(object? sender, RoutedEventArgs e){
      Console.WriteLine("Clear the grid.");
      PwdGrid.Reset();
      HidePatternCheckBox.IsChecked = false;
    }
    
    public void SetHidePattern(object? sender, RoutedEventArgs e){
      Console.WriteLine("Setting Hide Pattern bool");
      PwdGrid.IsPatternHidden = (sender as CheckBox).IsChecked ?? false;
      System.Console.WriteLine($"isPatternHidden: {PwdGrid.IsPatternHidden}");
      PwdGrid.ForceRender();
    }

   private async void AddUppercaseChanged(object? sender, RoutedEventArgs e){
      
      PwdGrid.IsUppercase = (sender as CheckBox).IsChecked ?? false;
      PwdGrid.UpdatePassword();
   }

   private async void MultiHashCheckChanged(object? sender, RoutedEventArgs e){
      if (MultiHashCB.IsChecked ?? false){
         PwdGrid.MultiHash = (int)MultiHashUD.Value;
      }
      else{
         PwdGrid.MultiHash = 0;
      }
      PwdGrid.UpdatePassword();
   }

   private async void MultiHashChanged(object? sender, RoutedEventArgs e){
      if (MultiHashCB?.IsChecked ?? false){
         PwdGrid.MultiHash = (int)MultiHashUD.Value;
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
      PwdGrid.MaxLength = (int)MaxLengthUD.Value;
      PwdGrid.UpdatePassword();
      Console.WriteLine($"maxlength: {MaxLengthUD.Value}");
   }

   private async void SiteKeyChanged(object? sender, RoutedEventArgs e){
      //PwdGrid.SiteKey = (sender as ListBox).Text;
      Console.WriteLine($"{sender}  {(e as SelectionChangedEventArgs)}");
       if (sender is ListBox lb && lb.SelectedItem is string text)
       {
           Console.WriteLine($"Selected text: {text}");
           PwdGrid.SiteKey = text;
           if (MultiHashCB.IsChecked ?? false){
              PwdGrid.MultiHash = (int)MultiHashUD.Value;
           }
           else{
              PwdGrid.MultiHash = 0;
           }
           PwdGrid.UpdatePassword();
       }
   }

   private async void PasswordTextChanged(object? sender, RoutedEventArgs e){
         try{
            ClipboardService.SetText(PwdTextBox.Text);
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
      }
      else{
         Console.WriteLine("The data couldn't be decrypted. You may have used an incorrect password key or the data may be corrupted.");
      }
      
    }

    private async void AddTestSiteKeys(object? sender, RoutedEventArgs e){
         var vm = (MainWindowViewModel)DataContext;
        vm.allSiteKeys.Add("test1");
        vm.allSiteKeys.Add("test2");
        vm.allSiteKeys.Add("test3");
    }

   private async void DeleteSiteKey(object? sender, RoutedEventArgs e){
      var vm = (MainWindowViewModel)DataContext;
     Console.WriteLine($"{SiteKeys.SelectedItem}");
     bool isDeleted = vm.allSiteKeys.Remove($"{SiteKeys.SelectedItem}");
     Console.WriteLine($"isDeleted: ${isDeleted}");
     LoadAppConfig();
     // SiteKeys.ItemsSource = vm.allSiteKeys.Items; 

   }

   private async void OnAddSiteKeyClick(object? sender, RoutedEventArgs e){
      var vm = (MainWindowViewModel)DataContext;
      var msg = new SiteKeyMsgBox("Please type the SiteKey you'd like to add.");
       bool result =  await msg.ShowDialog<bool>(this);
       if (result)
       {
           // User clicked OK
            Console.WriteLine($"User selected OK: {msg.SiteKey}");
            if (!string.IsNullOrEmpty(msg.SiteKey)){
               vm.allSiteKeys.Add(msg.SiteKey);
            }
            SiteKeys.SelectedItem = msg.SiteKey;
            // initial value gets set
            if (!string.IsNullOrEmpty(SiteKeys.SelectedItem?.ToString())){
               PwdGrid.SiteKey = SiteKeys.SelectedItem.ToString();
               if (MultiHashCB.IsChecked ?? false){
                  PwdGrid.MultiHash = (int)MultiHashUD.Value;
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
