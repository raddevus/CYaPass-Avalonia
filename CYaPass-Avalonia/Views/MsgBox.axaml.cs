using Avalonia.Controls;
using Avalonia.Interactivity;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class MsgBox : Window
{
   public MsgBox(): this("default message") {
   }
    public MsgBox(string message, bool isCancelShown = false)
    {
        InitializeComponent();
        MessageText.Text = message;
        this.Opened += (_, __) => {};
        CancelBtn.IsVisible = isCancelShown;
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
