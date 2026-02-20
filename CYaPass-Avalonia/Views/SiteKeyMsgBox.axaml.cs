using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CYaPass_Avalonia.Views;

public partial class SiteKeyMsgBox : Window
{
   public string SiteKey {get;set;} = string.Empty;
   public SiteKeyMsgBox(): this("default message") {
   }
    public SiteKeyMsgBox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => { SiteKeyText.Focus(); };
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       // replace all white-space anywhere in string
       SiteKey = SiteKeyText?.Text?.Replace(" ", "") ?? string.Empty;

       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
