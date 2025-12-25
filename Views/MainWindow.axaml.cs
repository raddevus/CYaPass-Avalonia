using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CYaPass_Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ClearHandler(object? sender, RoutedEventArgs e){
      System.Console.WriteLine("Clear the grid.");
      PwdGrid.Reset();
    }
}
