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
            Items.Add(item);
            Console.WriteLine($"got it: {item}");
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

