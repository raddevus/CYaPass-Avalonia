using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class SiteKeyMsgBox : Window
{
   public SiteKey SiteKey {get;set;}
   public SiteKeyMsgBox(): this("default message") {
   }
    public SiteKeyMsgBox(string message, SiteKey siteKey = null)
    {
        InitializeComponent();
        MessageText.Text = message;
        if (siteKey != null){
           System.Console.WriteLine("siteKey is valid!!!");
           SiteKey = siteKey;
        }
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => { SiteKeyText.Focus(); };
         SiteKeyText.Text = SiteKey?.ToString();
    }


    protected override void OnOpened(EventArgs e){
       base.OnOpened(e);
      HasUppercase.IsChecked = SiteKey?.HasUpperCase ?? false;
      HasSpecialChars.IsChecked = SiteKey?.HasSpecialChars ?? false;
      HasMaxLength.IsChecked = SiteKey?.MaxLength >0;
      MaxLengthUD.Value = SiteKey?.MaxLength ?? 0;
      SiteKeyText.Text = SiteKey?.Key ?? string.Empty;
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       SiteKey = new();
       // replace all white-space anywhere in string
       SiteKey.Key = SiteKeyText?.Text?.Replace(" ", "") ?? string.Empty;
       SiteKey.HasSpecialChars = HasSpecialChars.IsChecked ?? false;
       SiteKey.HasUpperCase = HasUppercase.IsChecked ?? false;
       if (HasMaxLength.IsChecked ?? false){
         SiteKey.MaxLength = (int) MaxLengthUD.Value;
       }
       

       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
