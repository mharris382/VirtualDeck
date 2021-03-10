using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

public static class CollectionExtensions
{
    public static string PrintCollection<T>(this IEnumerable<T> collection, char separatorChar, string start = null, string end = null)
    {
        StringBuilder sb = new StringBuilder();
        if (start != null)
        {
            sb.Append(start);
        }

        foreach (var obj in collection)
        {
            sb.Append(obj);
            sb.Append(separatorChar);
        }

        if (end != null) sb.Append(end);
        return sb.ToString();
    }

    public static string AsRegExPickOne(this IEnumerable<string> collection)
    {
        var sb = new StringBuilder();
        sb.Append('[');
        foreach (var n in collection)
        {
            sb.Append($"({n})|");
        }

        sb.Remove(sb.Length - 1, 1);
        sb.Append(']');
        return sb.ToString();
    }
}