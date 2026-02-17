using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CYaPass_Avalonia.Models;

public class AppConfig{

   private string configFile;
   private const string configFileName = "cya.config";
   private const string defaultTransferUrl = "https://newlibre.com/LibreStore/";

   public string LastSelectedKey {get;set;} = string.Empty;
   public string TransferUrl {get;set;} = string.Empty;
   public bool MultiHashIsOn{get;set;}
   public int MultiHashCount {get;set;}

   public AppConfig(string configPath = "", string transferUrl = ""){
      if (configPath == string.Empty){
        configFile = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath),configFileName);
      }
      else{
         configFile = configPath;
      }
      if (transferUrl == string.Empty){
         TransferUrl = defaultTransferUrl;
      }
      else{
         TransferUrl = transferUrl;
      }
   }

   async public Task<bool> Save(){
      File.Delete(configFile);
      var output = JsonSerializer.Serialize(this);
      await File.AppendAllTextAsync(configFile, output);
     if (File.Exists(configFile)){
        Console.WriteLine("Success! Wrote file.");
        return true;
     }
     return false;
   }
}
