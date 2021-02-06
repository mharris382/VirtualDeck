using System;
using UnityEngine;

public class DirectRef : MonoBehaviour
{
    public CompositeHdriMap mapRef;
    public TMPro.TextMeshProUGUI text;

    public void Awake()
    {
        text.text = $"Memory Usage:{GC.GetTotalMemory(true)/1000}kb";
    }
}