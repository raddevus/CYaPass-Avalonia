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
   void SetMultiHash(){
      AppConfig ac = new();
      ac.MultiHash = new AppConfig.MultiHashRecord(true,5);
      var output = JsonSerializer.Serialize(ac);
      Console.WriteLine(output);
   }
   
   [Fact]
   void DisplayAppConfigJson(){
      AppConfig ac = new(){ LastSelectedKey="lastOne", TransferUrl="https://actionmobile.app/", MultiHash = new AppConfig.MultiHashRecord(true,3)};
      
      var output = JsonSerializer.Serialize(ac);
      Console.WriteLine(output);
   }

   [Fact]
   async  Task SaveBasicAppConfig(){

      AppConfig ac = new(){ LastSelectedKey="lastOne", TransferUrl="https://actionmobile.app/", MultiHash = new AppConfig.MultiHashRecord(true,3)};
      var targetDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var configFile = "cya.config";
      File.Delete(Path.Combine(targetDir, configFile));
      var output = JsonSerializer.Serialize(ac);
      await File.AppendAllTextAsync(Path.Combine(targetDir,configFile), output);
     if (File.Exists(Path.Combine(targetDir, configFile))){
        Console.WriteLine("Success! Wrote file.");
     }
   }
   
   [Fact]
   async Task ReadBasicAppConfig(){
      var targetDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var configFile = "cya.config";
      if (File.Exists(Path.Combine(targetDir,configFile))){
         var configJson = await File.ReadAllTextAsync(Path.Combine(targetDir,configFile));
         AppConfig? config = null;
         var ex = Record.Exception(() =>{
            config  = JsonSerializer.Deserialize<AppConfig>(configJson);
         });
         Assert.Null(ex);
         Console.WriteLine($"last key: {config.LastSelectedKey} transferUrl: {config.TransferUrl}");
      }
      else{
         Console.WriteLine($"Couldn't do the work, because test file doesn't exist: {Path.Combine(targetDir, configFile)}");
      }
   }
}
