using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CYaPass_Avalonia.Models;

public class AppConfig{

   private const string configFileName = "cya.config";
   private const string defaultTransferUrl = "https://newlibre.com/LibreStore/";
   [JsonIgnore]
   public static string ConfigFile{get; set;} = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath),configFileName);
   [JsonIgnore]
   private static string transferUrl {get;set;} = defaultTransferUrl;

   public string TransferUrl {
      get{ return transferUrl;}
      set{ transferUrl = value;}
   }
   public string LastSelectedKey {get;set;} = string.Empty;
   public bool MultiHashIsOn{get;set;}
   public int MultiHashCount {get;set;}
   public AppConfig() {}
  public AppConfig(string configFile ="", string transferUrl="",string lastSelectedKey="",
        bool multiHashIsOn = false, int multiHashCount=0){

      if (configFile != string.Empty){
        ConfigFile = configFile;
      }
      if (transferUrl != string.Empty){
         TransferUrl = transferUrl;
      }
     LastSelectedKey = lastSelectedKey;
     MultiHashIsOn = multiHashIsOn;
     MultiHashCount = multiHashCount;
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
