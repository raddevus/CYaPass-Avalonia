using CYaPass_Avalonia.Models;
using System.Collections.Generic;

namespace CYaPass_Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public SiteKeySet<string> allSiteKeys{get;set;}  = new();
}
