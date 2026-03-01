using Avalonia.Controls;
using Avalonia.Interactivity;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Views;

public partial class ExportMsgBox : Window
{
   public string MainToken {get;set;}
   private string currentPwd{get;set;}
   private string currentSiteKey{get;set;}

    public ExportMsgBox(string message, string pwd, string sitekey)
    {
        InitializeComponent();
        MessageText.Text = message;
        currentPwd = pwd;
        currentSiteKey = sitekey; 
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => {MainTokenText.Focus(); };
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       // replace all white-space anywhere in string
       MainToken = MainTokenText?.Text?.Replace(" ", "") ?? string.Empty;

       // ### RULE: MainToken Has to be 10 chars or more
       if (string.IsNullOrEmpty(MainToken) || MainToken.Length < 10){
          MessageText.Text = "The string must be at least 10 characters and not contain any spaces.";
          MainTokenText.Focus();
          return;
       }

          System.Console.WriteLine($"currentPwd: {currentPwd} - {MainToken}");
       // ### RULE: The MainToken is not allowed to contain any of the password
       if (currentPwd.Contains(MainToken)){
          // display problem & exit
          MessageText.Text = "Your MainToken cannot include any portion of your password.\nPlease try again.";
          MainTokenText.Focus();
          return;
       }

       // ### RULE: The MainToken is not allowed to contain any of the SiteKey
       if (MainToken.Contains(currentSiteKey)){
          // display problem & exit
          MessageText.Text = "Your MainToken cannot include any portion of the SiteKey that you are using to encrypt your data.\nPlease try again.";
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
