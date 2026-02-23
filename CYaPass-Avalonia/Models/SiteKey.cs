using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CYaPass_Avalonia.Models;

public class SiteKey : IComparable{
   // MaxLength = 0 - means no max length is set (64 is default) 
   [JsonConverter(typeof(FlexibleIntConverter))]
   public int MaxLength{get;set;}
   public bool HasSpecialChars{get;set;}
   public bool HasUpperCase{get;set;}
   [JsonConverter(typeof(Base64ToStringConverter))]
   public string Key{get;set;} = string.Empty;

   public override string ToString(){
      return this.Key;
   }

    public int CompareTo(Object? other)
    {
        if (other == null) return 1;
        return string.Compare(this.ToString(), other.ToString(), StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
        return obj is SiteKey other &&
               string.Equals(this.Key, other.Key, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Key?.GetHashCode(StringComparison.Ordinal) ?? 0;
    }

}
