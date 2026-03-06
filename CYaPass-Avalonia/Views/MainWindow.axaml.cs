using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;           // For Application
using Avalonia.Styling;   // For ThemeVariant
using Avalonia.Media;     // For Brushes
using Avalonia.Input;     // For KeyEventArgs
using Avalonia.Utilities; // For Scrollview on ListBox
using Avalonia.Controls;
using Avalonia.Interactivity;
using NewLibre.Services;
using NewLibre.Models;
using CYaPass_Avalonia.ViewModels;
using CYaPass_Avalonia.Models;
using AppHelpers;

namespace CYaPass_Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closing += async (s,e)  =>  { 
           await SaveConfig();
        };
        InitThemeChangeHandler();
    }

    protected override void OnOpened(EventArgs e){
       base.OnOpened(e);
       var vm = (MainWindowViewModel)DataContext;

        CheckThemeVariant(); 
         vm.allSiteKeys.LoadFromFile();
        LoadAppConfig();
        InitSiteKeysScroll();
    }

    private void CheckThemeVariant(){
       Console.WriteLine($"theme: {ActualThemeVariant}"); 
      if (ActualThemeVariant == ThemeVariant.Dark)
      {
          // Apply dark mode background color
          LeftBorder.Background = Brushes.DarkBlue;
          RightBorder.Background = Brushes.DarkGreen;
      }
      else
      {
          // Apply light mode background color
          LeftBorder.Background = Brushes.LightBlue;
          RightBorder.Background = Brushes.LightYellow;
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

   private void InitSiteKeysScroll(){

        // Sets SiteKeys ListBox so keyboard will move
        // thru the List items easily
      SiteKeys.GetObservable(ListBox.ScrollProperty)
          .OfType<ScrollViewer>()
          .Take(1)
          .Subscribe(scrollViewer =>
          {
              scrollViewer.GetObservable(ScrollViewer.ScrollChangedEvent)
                  .Subscribe(_ =>
                  {
                  if (SiteKeys != null){
                      if (SiteKeys.SelectedIndex >= 0)
                      {
                          var item = (ListBoxItem)SiteKeys.ContainerFromIndex(SiteKeys.SelectedIndex)!;
                          item?.Focus(NavigationMethod.Directional);
                      }
                      }});
          });   
   }
    async public void LoadAppConfig(){
      
      var cfile = AppConfig.ConfigFile;  
      if (File.Exists(cfile)){
         try{
            var configJson = await File.ReadAllTextAsync(cfile);
            Console.WriteLine($" got it!!!!! => {configJson}");
            var lc = JsonSerializer.Deserialize<AppConfig>(configJson);
           var vm = (MainWindowViewModel)DataContext;
           // Sets ViewModel CyaConfig so we can get values later
           vm.CyaConfig = lc;
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
       // First save the SiteKeys
      vm?.allSiteKeys.Save();
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
          SiteKey currentKey = vm.allSiteKeys.GetItemByKey(lb.SelectedItem.ToString());
           Console.WriteLine($"Selected text: {text}");
           MaxLengthCB.IsChecked = currentKey.MaxLength > 0;
           MaxLengthUD.Value = currentKey.MaxLength;
           AddUppercaseCB.IsChecked = currentKey.HasUpperCase;
           SpecialCharsCB.IsChecked = currentKey.HasSpecialChars;
           PwdGrid.SiteKey = text;
           if (MultiHashCB.IsChecked ?? false){
              PwdGrid.MultiHash = (int)(MultiHashUD.Value ?? 0);
           }
           else{
              PwdGrid.MultiHash = 0;
           }
           PwdGrid.UpdatePassword();
       }
       if (SiteKeys != null){
          if (IsVisible && SiteKeys.SelectedIndex >= 0)
          {
           var selectedListBoxItem = (ListBoxItem)SiteKeys.ContainerFromIndex(SiteKeys.SelectedIndex)!;
           selectedListBoxItem?.Focus(NavigationMethod.Directional);
           }
        }
   }

   Avalonia.Input.Key currentKey;
   int startItem = 0;
   private void SiteKeys_KeyDown(object? sender, KeyEventArgs e){

      Console.WriteLine($"key: {e.Key}");
      Console.WriteLine($"startItem: {startItem}");
      if (currentKey != e.Key){startItem = 0;}
      currentKey = e.Key;
      
      Console.WriteLine($"key: {currentKey}");
      

      var vm = (MainWindowViewModel)DataContext;
      for (int i = startItem; i < vm.allSiteKeys.Items.Count; i++){
        var currentItem = vm.allSiteKeys.Items[i] as string ?? "";
        if (currentItem.StartsWith(e.Key.ToString(), StringComparison.OrdinalIgnoreCase)){
           SiteKeys.ScrollIntoView(i-1);
           SiteKeys.Focus();
           SiteKeys.SelectedIndex = i;
           SiteKeys.Focus();
           startItem = i+1;

           SiteKeys.SelectedIndex = i;


           var selectedListBoxItem = (ListBoxItem)SiteKeys.ContainerFromIndex(SiteKeys.SelectedIndex)!;
           selectedListBoxItem?.Focus(NavigationMethod.Directional);
           break;
        }
        else{ startItem = 0;} // didn't find
      }
   }

   private async void PasswordTextChanged(object? sender, RoutedEventArgs e){
         try{
            var clipboard = AppHelpers.Clipboard.Get();
            if (clipboard != null) {
               clipboard.SetTextAsync(PwdTextBox?.Text ?? string.Empty);
            }
         }
         catch (Exception ex){
            Console.WriteLine("Couldn't copy to clipboard.");
         }
   }

    async public void ExportSiteKeys(object? sender, RoutedEventArgs e){

       // Insure that the user has a password generated otherwise exit
       var currentPwd = PwdGrid.FullLengthPassword;
       if (string.IsNullOrEmpty(currentPwd)){ 
          new MsgBox("You must select a SiteKey & draw a pattern (generate a password) to export your SiteKeys.\nPlease try again.").ShowDialog<bool>(this);
          return;
       }       
      var msg = new ExportMsgBox("Please type your MainToken Key that will be used to store your data.", currentPwd, SiteKeys.SelectedItem.ToString());
       bool dialogResult =  await msg.ShowDialog<bool>(this);
       var mainToken = msg.MainToken;
       Console.WriteLine($"mainToken: {mainToken}  : result {dialogResult}");
       var vm = (MainWindowViewModel)DataContext;
       if (dialogResult)
       {
         if (string.IsNullOrEmpty(mainToken)){ return;}
         Console.WriteLine("Exporting SiteKeys...");
         Console.WriteLine($"{msg.MainToken} : {vm.CyaConfig.TransferUrl}");         
         try {
               string ivFromEncrypt;
               // 1. Call EncryptSiteKeys - get encrypted & encoded data back 
               var encryptDto = await vm.allSiteKeys.EncryptSiteKeys(currentPwd); 
               // Console.WriteLine($"dto: {encryptDto}");
               // 2. generate HMAC from data & iv
               Crypton c = new();
               var hmac = c.GenerateHmac(currentPwd,$"{encryptDto.iv}:{encryptDto.data}");
               Console.WriteLine($"hmac: {hmac}");
               // 3. post maintoken, data, hmac, iv to SaveData
               CyaService cyasvc = new(mainToken, vm.CyaConfig.TransferUrl);
               var result = await cyasvc.SaveCyaData(encryptDto.data, hmac, encryptDto.iv);

               if (result){
                  new MsgBox("Successfully exported keys.");
                  return;
               }
            new MsgBox("Couldn't export keys.\nTry again.");

         }
         catch (Exception ex){
         }
       }
    }
    
    async public void SetTransferUrl(object? sender, RoutedEventArgs e){

        var vm = (MainWindowViewModel)DataContext;
        // Sends in the current value of the TransferUrl
        // so it can be displayed in the dialog
       var msg = new SetUrlMsgBox(vm.CyaConfig.TransferUrl, "Please set the URL to the location where you have LibreStore running.\nOr set to default to store on public LibreStore.");
       bool result =  await msg.ShowDialog<bool>(this);
       var transferUrl = msg.TransferUrl;

       Console.WriteLine($" from msgbox: {transferUrl}");
       if (result)
       {
          vm.CyaConfig.TransferUrl = transferUrl;
          vm.CyaConfig.Save();
          Console.WriteLine($"new URL: {vm.CyaConfig.TransferUrl}");
       }

    }

    async public void ImportSiteKeys(object? sender, RoutedEventArgs e){
      var msg = new ImportMsgBox("Please type your MainToken Key that will be used to retrieve your data.");
       var mainToken = msg.MainToken;
       var vm = (MainWindowViewModel)DataContext;
       bool dialogResult =  await msg.ShowDialog<bool>(this);
       if (dialogResult)
       {
         if (!string.IsNullOrEmpty(mainToken)){ return;}
         Console.WriteLine("Importing SiteKeys...");
         Console.WriteLine($"{msg.MainToken} : {vm.CyaConfig.TransferUrl}");         
         var cyasvc = new CyaService(msg.MainToken,vm.CyaConfig.TransferUrl);
         try {
            var result = await cyasvc.GetCyaData();
            var encryptedSiteKeys = result.CyaBucket.Data;
            var iv = result.CyaBucket.Iv;
            Console.WriteLine($"{encryptedSiteKeys}");
            Crypton c = new();
            if (!c.ValidateHmac(PwdGrid.FullLengthPassword, $"{iv}:{encryptedSiteKeys}", result.CyaBucket.Hmac)){
               new MsgBox("The hmac did not match! Data is corrupted or hacked. Cannot decrypt. \n Make sure you're using correct MainToken & Password & try again.").ShowDialog<bool>(this);
               return;
            }
            string decryptedData = string.Empty;
            var isSuccessDecrypt = c.Decrypt(encryptedSiteKeys, PwdGrid.FullLengthPassword, iv, out decryptedData);
            if (isSuccessDecrypt){
               Console.WriteLine($"{decryptedData}"); 
               var downloadedSiteKeys = JsonSerializer.Deserialize<List<SiteKey>>(decryptedData);
               int importKeyCount = 0;
               foreach (SiteKey s in downloadedSiteKeys){ 
                 if(vm.allSiteKeys.Add(s)){
                  // only increment if the key was added
                  importKeyCount++;
                 }
               } 
               // Imported SiteKeys so lets save the local file
               vm.allSiteKeys.Save();
               new MsgBox($"Imported {importKeyCount} new SiteKey(s)").ShowDialog<bool>(this);
            }
            else{
               new MsgBox("The data couldn't be decrypted. You may have used an incorrect password key or the data may be corrupted.").ShowDialog<bool>(this);
            }
         } // try
         catch (Exception ex){
            new MsgBox($"The import failed. Could not find the MainToken.\nCheck the value you are using & try again.\n {ex.Message}").ShowDialog<bool>(this);
         }
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
     var removeMsgBox = new MsgBox("Are you sure you want to delete the SiteKey?", true);
     var result = await removeMsgBox.ShowDialog<bool>(this);
     if (!result){ return;}
     bool isDeleted = vm.allSiteKeys.Remove(new SiteKey{Key=SiteKeys.SelectedItem.ToString()});
     if (isDeleted){
        // removed the item, so save sitekeys to file
        vm.allSiteKeys.Save();
     }
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
               // New SiteKey was added so save all to file
               vm.allSiteKeys.Save();
            }
            SiteKeys.SelectedItem = msg.SiteKey.Key;
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

   private async void RemoveAllSiteKeys(object? sender, RoutedEventArgs e){
         var vm = (MainWindowViewModel)DataContext;
         var removeMsgBox = new MsgBox("Are you sure you want to Remove all SiteKeys from this system?\n\nIf you continue (click the [OK] button) all of the SiteKeys will be removed and the file where they exist will be deleted.\n\nMake sure you have a way to restore them before continuing.", true);
         var result = await removeMsgBox.ShowDialog<bool>(this);
         if (!result){ return;}
         // Remove all Items from ListBox
         vm.allSiteKeys.Items.Clear();
         vm.allSiteKeys = new();
         SiteKeys.ItemsSource = null;
         SiteKeys.ItemsSource = vm.allSiteKeys.Items;
         // Delete the file so it no longer exists
         vm.allSiteKeys.DeleteSiteKeyFile();
   }
}
