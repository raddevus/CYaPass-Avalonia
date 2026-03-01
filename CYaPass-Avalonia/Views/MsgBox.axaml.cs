using Avalonia.Controls;
using Avalonia.Interactivity;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class MsgBox : Window
{
   public MsgBox(): this("default message") {
   }
    public MsgBox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
        this.Opened += (_, __) => {};
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       Close(true);
    }
}
