using Godot;
using System;
using System.Collections.Generic;

public partial class Debug : Node
{
    public static Color InfoColor = Color.FromHtml("#2b9bf6");
    public static Color WarnColor = Color.FromHtml("#c68607");
    public static Color ErrColor = Color.FromHtml("#FF2000");
    public static void Info(string text)
    {
        GD.PrintRich("[color=" + InfoColor.ToHtml() +"][b][INFO][/b] " + text + "[/color]");
    }
    public static void Warn(string text)
    {
        GD.PrintRich("[color=" + WarnColor.ToHtml() +"][b][WARN][/b] " + text + "[/color]");
    }
    public static void Err(string text)
    {
        GD.PrintRich("[color=" + ErrColor.ToHtml() +"][b][ERR!][/b] " + text + "[/color]");
    }
    public static void Info(string[] textArray)
    {
        string text = "";
        for(int i = 0; i < textArray.Length; i++)
        {
            text += textArray[i] + ", ";
        }
        GD.PrintRich("[color=" + InfoColor.ToHtml() +"][b][INFO][/b] " + text + "[/color]");
    }
    public static void Warn(string[] textArray)
    {
        string text = "";
        for(int i = 0; i < textArray.Length; i++)
        {
            text += textArray[i] + ", ";
        }
        GD.PrintRich("[color=" + WarnColor.ToHtml() +"][b][WARN][/b] " + text + "[/color]");
    }
    public static void Err(string[] textArray)
    {
        string text = "";
        for(int i = 0; i < textArray.Length; i++)
        {
            text += textArray[i] + ", ";
        }
        GD.PrintRich("[color=" + ErrColor.ToHtml() +"][b][ERR!][/b] " + text + "[/color]");
    }
}
