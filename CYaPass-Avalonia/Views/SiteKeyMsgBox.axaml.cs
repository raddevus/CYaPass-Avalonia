using Avalonia.Controls;
using Avalonia.Interactivity;
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

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       SiteKey = new();
       // replace all white-space anywhere in string
       SiteKey.Key = SiteKeyText?.Text?.Replace(" ", "") ?? string.Empty;

       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
