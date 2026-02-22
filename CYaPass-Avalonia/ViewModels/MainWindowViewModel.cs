using CYaPass_Avalonia.Models;
using System.Collections.Generic;

namespace CYaPass_Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public SiteKeySet<SiteKey> allSiteKeys{get;set;}  = new();
    public AppConfig CyaConfig{get;set;} = new();
}
