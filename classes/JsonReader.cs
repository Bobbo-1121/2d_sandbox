using Godot;
using System;

public class JsonReader
{
    public class NullInt
    {
        public int value;
    }
    public static string LoadFromPath(string path)
    {
        if (path.Substr(path.Length - 4, 4) != "json")
        {
            return null;
        }
        FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        return file.GetAsText();
    }
    public static Godot.Collections.Dictionary<string, Variant> StringToDictionary(string jsonString)
    {
        Json json = new();
        Error error = json.Parse(jsonString);
        if (error != Error.Ok)
        {
            return null;
        }
        return json.Data.AsGodotDictionary<string, Variant>();
    }
    public static NullInt ReadInt(Godot.Collections.Dictionary<string, Variant> dataDictionary, string key)
    {
        if (!dataDictionary.ContainsKey(key))
        {
            return null;
        }
        Variant value = dataDictionary[key];
        if (value.VariantType != Variant.Type.Int)
        {
            return null;
        }
        return new()
        {
            value = value.AsInt32()
        };
    }
    public static string ReadString(Godot.Collections.Dictionary<string, Variant> dataDictionary, string key)
    {
        if (!dataDictionary.ContainsKey(key))
        {
            return null;
        }
        Variant value = dataDictionary[key];
        if (value.VariantType != Variant.Type.String)
        {
            return null;
        }
        return value.AsString();
    }
}