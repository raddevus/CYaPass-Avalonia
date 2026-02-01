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

   private async void OnAddSiteKeyClick(object? sender, RoutedEventArgs e){
      var msg = new SiteKeyMsgBox("Please type the SiteKey you'd like to add.");
    bool result =  await msg.ShowDialog<bool>(this);

    if (result)
    {
        // User clicked OK
         System.Console.WriteLine($"User selected OK: {msg.SiteKey}");

    }
    else
    {
        // User clicked Cancel
        System.Console.WriteLine("User cancelled");
    }
    }

}
