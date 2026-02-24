using Avalonia.Controls;
using Avalonia.Interactivity;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class SetUrlMsgBox : Window
{
   public string TransferUrl {get;set;}
   public SetUrlMsgBox(): this(string.Empty,"default message") {
   }
    public SetUrlMsgBox(string transferUrl, string message)
    {
        InitializeComponent();
        MessageText.Text = message;
        TransferUrl = TransferUrlText?.Text = transferUrl;
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => {TransferUrlText.Focus(); };
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       // replace all white-space anywhere in string
       TransferUrl = TransferUrlText?.Text?.Replace(" ", "") ?? string.Empty;
       System.Console.WriteLine($"url in msg textbox: {TransferUrl}");
       if (string.IsNullOrEmpty(TransferUrl) || !TransferUrl.Contains("http") || !TransferUrl.Contains("://")){
          MessageText.Text = "The string must not contain any spaces and must be a valid URL.";
          TransferUrlText.Focus();
          return;
       }
       Close(true);
    }

    private void SetDefault(object? sender, RoutedEventArgs e){
       // Default value is to NewLibre/LibreStore
      TransferUrl = TransferUrlText.Text = "https://newlibre.com/LibreStore/";
    }
    
    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
