// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoAsync._sunamo;

internal class SH
{
    internal static string JoinNL(List<string> lines)
    {
        StringBuilder stringBuilder = new();
        foreach (var item in lines) stringBuilder.AppendLine(item);
        var result = string.Empty;
        result = stringBuilder.ToString();
        return result;
    }
    internal static List<string> SplitChar(string text, params char[] dot)
    {
        return text.Split(dot, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    internal static List<string> Split(string text, params string[] dot)
    {
        return text.Split(dot, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    internal static List<string> SplitNone(string text, params string[] deli)
    {
        return text.Split(deli, StringSplitOptions.None).ToList();
    }
    internal static string NullToStringOrDefault(object value)
    {

        return value == null ? " " + "(null)" : " " + value;
    }
    internal static string TrimEnd(string name, string ext)
    {
        while (name.EndsWith(ext)) return name.Substring(0, name.Length - ext.Length);
        return name;
    }
}