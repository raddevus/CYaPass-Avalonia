using Avalonia.Controls;
using Avalonia.Interactivity;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class ExportMsgBox : Window
{
   public string MainToken {get;set;}
   public ExportMsgBox(): this("default message") {
   }
    public ExportMsgBox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => {MainTokenText.Focus(); };
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       // replace all white-space anywhere in string
       MainToken = MainTokenText?.Text?.Replace(" ", "") ?? string.Empty;
       if (string.IsNullOrEmpty(MainToken) || MainToken.Length < 10){
          MessageText.Text = "The string must be at least 10 characters and not contain any spaces.";
          MainTokenText.Focus();
          return;
       }
       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
