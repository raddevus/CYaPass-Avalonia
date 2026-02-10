using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace CYaPass_Avalonia.Models;

public class SiteKeySet<T>
{
    private readonly HashSet<T> _set = new();
    public ObservableCollection<T> Items{get;}  = new();
    
    public bool Add(T item)
    {
        if (_set.Add(item))
        {
           // Find the correct insertion index 
           int index = 0; 
           while (index < Items.Count && 
                 Comparer<T>.Default.Compare(Items[index], item) < 0){
              index++;
           }
            Items.Insert(index, item);
            return true;
        }
        return false;
    }

    public bool Remove(T item)
    {
        if (_set.Remove(item))
        {
            Items.Remove(item);
            Console.WriteLine($"Items.Count: {Items.Count}");
            return true;
        }
        return false;
    }

    public bool Contains(T item) => _set.Contains(item);
}

