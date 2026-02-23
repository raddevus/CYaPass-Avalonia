using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CYaPass_Avalonia.Models;

public class Base64ToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var encoded = reader.GetString();
        if (string.IsNullOrEmpty(encoded))
            return string.Empty;

        try
        {
            var bytes = Convert.FromBase64String(encoded);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            // If it's not Base64, just return the raw string
            return encoded;
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        writer.WriteStringValue(Convert.ToBase64String(bytes));
    }
}

