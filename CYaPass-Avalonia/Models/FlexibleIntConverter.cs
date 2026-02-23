using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CYaPass_Avalonia.Models;
public class FlexibleIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (int.TryParse(s, out var value))
                return value;
        }

        throw new JsonException($"Invalid int value: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

