using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CYaPass_Avalonia.Models;

public class AppConfig{

   public string LastSelectedKey {get;set;} = string.Empty;
   public string TransferUrl {get;set;} = string.Empty;
   public bool MultiHashIsOn{get;set;}
   public int MultiHashCount {get;set;}

   async public Task<bool> Save(){

      AppConfig ac = new(){ LastSelectedKey="lastOne", TransferUrl="https://actionmobile.app/", MultiHashIsOn = true, MultiHashCount = 3};
      var targetDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var configFile = "cya.config";
      File.Delete(Path.Combine(targetDir, configFile));
      var output = JsonSerializer.Serialize(ac);
      await File.AppendAllTextAsync(Path.Combine(targetDir,configFile), output);
     if (File.Exists(Path.Combine(targetDir, configFile))){
        Console.WriteLine("Success! Wrote file.");
        return true;
     }
     return false;
   }
}
