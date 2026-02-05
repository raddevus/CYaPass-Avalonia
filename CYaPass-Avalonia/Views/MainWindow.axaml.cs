using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TextCopy;
using NewLibre.Services;
using NewLibre.Models;

namespace CYaPass_Avalonia.Views;

public partial class MainWindow : Window
{
    HashSet<string> allSiteKeys = new();
    public MainWindow()
    {
        InitializeComponent();
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

   private async void SiteKeyChanged(object? sender, RoutedEventArgs e){
      //PwdGrid.SiteKey = (sender as ListBox).Text;
      Console.WriteLine($"{sender}  {(e as SelectionChangedEventArgs)}");
       if (sender is ListBox lb && lb.SelectedItem is string text)
       {
           Console.WriteLine($"Selected text: {text}");
           PwdGrid.SiteKey = text;
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

   private async void OnAddSiteKeyClick(object? sender, RoutedEventArgs e){
      var msg = new SiteKeyMsgBox("Please type the SiteKey you'd like to add.");
    bool result =  await msg.ShowDialog<bool>(this);

    if (result)
    {
        // User clicked OK
         Console.WriteLine($"User selected OK: {msg.SiteKey}");
         if (!string.IsNullOrEmpty(msg.SiteKey)){
            allSiteKeys.Add(msg.SiteKey);
         }
         SiteKeys.ItemsSource = allSiteKeys.ToArray().OrderBy(x => x);
         // Control has has one item selected so we make sure the
         // initial value gets set
         if (allSiteKeys.Count == 1){
            PwdGrid.SiteKey = allSiteKeys.ToArray()[0];
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
