using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MessageLog : MonoBehaviour
{
    [Required, AssetsOnly] public TextMeshProUGUI msgPrefab;

    [Required, ChildGameObjectsOnly] public Transform logParent;

    public int maxNumberOfLogs;
    public bool orderNewLogsAsFirstChild;


    Queue<TextMeshProUGUI> activeLogs = new Queue<TextMeshProUGUI>();

    public void OnAwake()
    {
        activeLogs = new Queue<TextMeshProUGUI>();
    }
    
    private void OnApplicationQuit()
    {
        activeLogs.Clear();
    }

    public void LogMessage(string msg)
    {
        var log = GetNextLog();
        log.text = msg;
        activeLogs.Enqueue(log);
        log.transform.parent = logParent;
        
        if(orderNewLogsAsFirstChild)
            log.transform.SetAsFirstSibling();
        else
            log.transform.SetAsLastSibling();
    }


    public void ClearLog()
    {
        foreach (var log in activeLogs)
        {
            Destroy(log.gameObject);
        }
    }


    TextMeshProUGUI GetNextLog()
    {
        TextMeshProUGUI txt = null;
        if (maxNumberOfLogs > 0 && activeLogs.Count >= maxNumberOfLogs)
        {
            txt = activeLogs.Dequeue();
        }
        else
        {
            txt = Instantiate(msgPrefab);
        }
        return txt;
    }
}