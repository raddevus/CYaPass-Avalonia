using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CYaPass_Avalonia.Models;

public class AppConfig{

   private const string configFileName = "cya.config";
   private const string defaultTransferUrl = "https://newlibre.com/LibreStore/";
   public static string ConfigFile{get; set;} = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath),configFileName);
   public static string TransferUrl {get;set;} = defaultTransferUrl; 

   public string LastSelectedKey {get;set;} = string.Empty;
   public bool MultiHashIsOn{get;set;}
   public int MultiHashCount {get;set;}

   public AppConfig(string configPath = "", string transferUrl = ""){
      Console.WriteLine("extra ctor is running...");
      if (configPath != string.Empty){
        ConfigFile = configPath;
      }
      if (transferUrl != string.Empty){
         TransferUrl = transferUrl;
      }
   }

   async public Task<bool> Save(){
      File.Delete(ConfigFile);
      var output = JsonSerializer.Serialize(this);
      await File.AppendAllTextAsync(ConfigFile, output);
     if (File.Exists(ConfigFile)){
        Console.WriteLine("Success! Wrote file.");
        return true;
     }
     return false;
   }
}
