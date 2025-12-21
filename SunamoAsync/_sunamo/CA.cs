namespace SunamoAsync._sunamo;

internal class CA
{
    internal static bool IsListStringWrappedInArray<T>(List<T> list)
    {
        var first = list.First().ToString();
        if (list.Count == 1 && (first == "System.Collections.Generic.List`1[System.String]" ||
        first == "System.Collections.Generic.List`1[System.Object]")) return true;
        return false;
    }
    internal static void InitFillWith(List<string> list, int count, string initWith = "")
    {
        InitFillWith<string>(list, count, initWith);
    }
    internal static void InitFillWith<T>(List<T> list, int count, T initWith)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(initWith);
        }
    }
    internal static void InitFillWith<T>(List<T> list, int count)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(default);
        }
    }
}