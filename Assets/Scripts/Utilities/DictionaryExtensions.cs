using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="valueToAdd"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns>True if the value did not exist and was therefore added, false if the value did already exist</returns>
    public static bool AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue valueToAdd)
    {
        if (dict.ContainsKey(key))
            return false;
            
        dict.Add(key, valueToAdd);
        return true;
    }

    public static bool ModifyIfExists<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue, TValue> modifyFunction)
    {
        if (!dict.ContainsKey(key))
        {
            return false;
        }

        dict[key] = modifyFunction(dict[key]);
        return true;
    }

    public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue valueToReplace)
    {
        if (dict.ContainsKey(key))
            dict[key] = valueToReplace;
        else
            dict.Add(key, valueToReplace);
    }
    public static void AddOrModify<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue, TValue> modifyFunction, TValue valueToReplace)
    {
        if (dict.ContainsKey(key))
            dict[key] = modifyFunction(dict[key]);
        else
            dict.Add(key, valueToReplace);
    }


    public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
        foreach (var value in values)
        {
            dict.AddOrReplace(value.Key, value.Value);
        }
    }
}