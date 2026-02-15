using System.Text.Json;
using CYaPass_Avalonia.Models;

namespace CYaPass_Avalonia.Tests;

public class AppConfigTests{

   [Fact]
   void DisplayEmptyAppConfigJson(){
      AppConfig ac = new();
      var output = JsonSerializer.Serialize(ac);
      Console.WriteLine(output);
      
   }

   [Fact]
   void DisplayAppConfigJson(){
      AppConfig ac = new(){ LastSelectedKey="lastOne", TransferUrl="https://actionmobile.app/", MultiHash = new AppConfig.MultiHashRecord(true,3)};
      
      var output = JsonSerializer.Serialize(ac);
      Console.WriteLine(output);
   }
}
