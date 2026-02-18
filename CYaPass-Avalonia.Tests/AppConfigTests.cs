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
      ac.MultiHashIsOn = true;
      ac.MultiHashCount = 5;
      var output = JsonSerializer.Serialize(ac);
      Console.WriteLine(output);
   }
   
   [Fact]
   void DisplayAppConfigJson(){
      AppConfig ac = new(){ LastSelectedKey="lastOne", MultiHashIsOn = true, MultiHashCount=7};
      
      var output = JsonSerializer.Serialize(ac);
      Console.WriteLine(output);
   }

   [Fact]
   async  Task SaveBasicAppConfig(){

      AppConfig.ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "cya.config");
      AppConfig ac = new(){ LastSelectedKey="lastOne", MultiHashIsOn=true, MultiHashCount=3};
      File.Delete(AppConfig.ConfigFile);
      var output = JsonSerializer.Serialize(ac);
      await File.AppendAllTextAsync(AppConfig.ConfigFile, output);
     if (File.Exists(AppConfig.ConfigFile)){
        Console.WriteLine("Success! Wrote file.");
     }
   }
   
   [Fact]
   async Task ReadBasicAppConfig(){
      AppConfig ac = new();
      AppConfig.ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "cya.config");
      if (File.Exists(AppConfig.ConfigFile)){
         var configJson = await File.ReadAllTextAsync(AppConfig.ConfigFile);
         Console.WriteLine($" got it!!!!! => {configJson}");
         var ex = Record.Exception(() =>{
             ac = JsonSerializer.Deserialize<AppConfig>(configJson);
         });
         Assert.Null(ex); 
         Console.WriteLine($"last key: {ac.LastSelectedKey} transferUrl: {new AppConfig().TransferUrl}");
      }
      else{
         Console.WriteLine($"Couldn't do the work, because test file doesn't exist: {AppConfig.ConfigFile}");
      }
   }
}
