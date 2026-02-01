using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CYaPass_Avalonia.Views;

public partial class SiteKeyMsgBox : Window
{
   public string SiteKey {get;set;}
   public SiteKeyMsgBox(): this("default message") {
   }
    public SiteKeyMsgBox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
       
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       SiteKey = SiteKeyText.Text;
       //SiteKey = System.IO.Path.GetRandomFileName();

       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
