using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NewLibre.Services;
using NewLibre.Models;

namespace CYaPass_Avalonia.Models;

public class SiteKeySet<SiteKey>
{
    private readonly HashSet<SiteKey> _set = new();
    public ObservableCollection<string> Items{get;}  = new();

   [JsonIgnore]
   public string SiteKeyPath{get; set;} = Path.GetDirectoryName(Environment.ProcessPath);
    private string SiteKeyFile = "sitekeys.json";
    
    public bool Add(SiteKey item)
    {
       Console.WriteLine($"Add : {item}");
       
        if (!Contains(item))
        {
           _set.Add(item);
           // Find the correct insertion index 
            var index = GetItemIndex(item);
           
            Items.Insert(index, item.ToString());
            return true;
        }
        return false;
    }

   public SiteKey GetItemByKey(String key){
      int index = 0;
      var allHashes = _set.ToList();
      while (index < Items.Count){
         if (allHashes[index].ToString() == key){
            return allHashes[index];
         }
         index++;
      }
      return default(SiteKey);
   }

   private int GetItemIndex(SiteKey item ){
        int index = 0; 
         while (index < Items.Count &&
             string.Compare(Items[index], item.ToString(),
                StringComparison.CurrentCultureIgnoreCase) < 0){
            index++;
         }
         return index;
   }

    public bool Remove(SiteKey item)
    {
       Console.WriteLine($"Remove: {item}");

        if (_set.Remove(item))
        {
            Items.Remove(item.ToString());
            Console.WriteLine($"Items.Count: {Items.Count}");
            return true;
        }
        return false;
    }

    public bool Contains (SiteKey item) {
       Console.WriteLine($"contains: {item} - {item.ToString()}");
       return Items.Contains(item.ToString());
    }

    async public Task<bool> Save(){
      var targetFile = Path.Combine(SiteKeyPath,SiteKeyFile);
      File.Delete(targetFile);
      var output = JsonSerializer.Serialize(_set);
      await File.AppendAllTextAsync(targetFile, output);
     if (File.Exists(targetFile)){
        Console.WriteLine($"Success! Wrote SiteKey file. {targetFile}");
        return true;
     }
     return false;
   }

    async public Task<EncryptDto> EncryptSiteKeys(string pwd){
      var targetFile = Path.Combine(SiteKeyPath,SiteKeyFile);
      Console.WriteLine($"targetFile: {targetFile}");
      var allKeys = await File.ReadAllTextAsync(targetFile);
      Console.WriteLine("read file.");
      Crypton c = new();
      string iv = string.Empty;
      var encData =  c.Encrypt(allKeys, pwd, out iv); 
      var outData = new EncryptDto(encData, iv);
      Console.WriteLine($"encData: {encData}");
      return outData; 
    }

   async public Task<bool> LoadFromFile(){
      
      var targetFile = Path.Combine(SiteKeyPath,SiteKeyFile);
      Console.WriteLine($"targetFile : {targetFile}");
      var allKeys = await File.ReadAllTextAsync(targetFile);
      Console.WriteLine("deserializing...");
      var keys =  JsonSerializer.Deserialize<List<SiteKey>>(allKeys);
      Console.WriteLine($"keys: {keys}");
      foreach (SiteKey k in keys){ Add(k);} 
      return true;
   }

   public void DeleteSiteKeyFile(){
      var targetFile = Path.Combine(SiteKeyPath,SiteKeyFile);
      Console.WriteLine($"Deleting: {targetFile}");
      File.Delete(targetFile);

   }
}
public record EncryptDto (string encryptedData, string iv);
