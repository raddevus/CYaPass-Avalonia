
namespace CYaPass_Avalonia.Models;

public class SiteKey{
   // MaxLength = 0 - means no max length is set (64 is default) 
   public int MaxLength{get;set;}
   public bool HasSpecialChars{get;set;}
   public bool HasUpperCase{get;set;}
   public string Key{get;set;} = string.Empty;
}
