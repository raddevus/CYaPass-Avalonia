
namespace CYaPass_Avalonia.Models;

public class AppConfig{

   public string LastSelectedKey {get;set;}
   public string TransferUrl {get;set;}
   public record MultiHash( bool MultiHashIsOn, int MultiHashCount);
}
