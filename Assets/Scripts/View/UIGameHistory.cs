using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIGameHistory : MonoBehaviour
{
    [AssetsOnly]
    public TMPro.TextMeshProUGUI textElementPrefab;


    public Transform eventHistoryParent;


    private void Awake()
    {
        MessageBroker.Default.Receive<NewTurnMessage>().TakeUntilDestroy(this).Subscribe(_ =>
        {
            LogNewTurn();
        });
        MessageBroker.Default.Receive<CardPlayedMessage>().TakeUntilDestroy(this).Subscribe(t =>
        {
            LogCardPlayed(t.CardInstance);
        });
    }


    public void LogCardPlayed(CardInstance cardInstance)
    {
        string s = $"<b><color=green>Played {cardInstance.Card.Name}</color> </b>";
        LogEvent(s);
    }

    private int turnCount = 0;
    public void LogNewTurn()
    {
        turnCount++;
        LogEvent("Started Turn: " + turnCount);
    }


    private void LogEvent(string msg)
    {
        var instance = Instantiate(textElementPrefab, eventHistoryParent);
        instance.text = msg;
        var scrollbar = GetComponentInChildren<Scrollbar>();
        if(scrollbar != null)
            scrollbar.value = 1;
    }
    
}