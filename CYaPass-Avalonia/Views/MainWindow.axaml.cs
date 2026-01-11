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
      HidePatternCheckBox.IsChecked = false;
    }
    
    public void SetHidePattern(object? sender, RoutedEventArgs e){
      System.Console.WriteLine("Setting Hide Pattern bool");
      PwdGrid.IsPatternHidden = (sender as CheckBox).IsChecked ?? false;
      System.Console.WriteLine($"isPatternHidden: {PwdGrid.IsPatternHidden}");
      PwdGrid.ForceRender();
    }
}
