using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CYaPass_Avalonia.Models;

public class SiteKeySet<SiteKey>
{
    private readonly HashSet<SiteKey> _set = new();
    public ObservableCollection<string> Items{get;}  = new();
    
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
}

