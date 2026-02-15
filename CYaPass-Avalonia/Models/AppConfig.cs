
namespace CYaPass_Avalonia.Models;

public class AppConfig{

   public string LastSelectedKey {get;set;}
   public string TransferUrl {get;set;}
   public MultiHashRecord MultiHash{get; set;}
   public record MultiHashRecord( bool MultiHashIsOn, int MultiHashCount);
}
