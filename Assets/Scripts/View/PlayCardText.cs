using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayCardText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private string lastCardPlayed;
    private int multiple;
    private string msgOG;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        MessageBroker.Default.Receive<NewTurnMessage>().TakeUntilDestroy(this).AsUnitObservable().Subscribe(_ =>
        {
            text.text = "";
        });

        MessageBroker.Default.Receive<CardPlayedMessage>().TakeUntilDestroy(this).Subscribe(c =>
        {
            var n = c.CardInstance.Card.name;
         
            if (n == lastCardPlayed)
            {
                multiple += 1;
                text.text = $"{msgOG}\n<b><color=yellow>x{multiple}</color></b>";
            }
            else
            {
                lastCardPlayed = n;
                msgOG = $"Played <color=red>{c.CardInstance.Card.name}</color> for {c.CardInstance.Card.ap} AP\n\n<b>{c.CardInstance.Card.description}</b>";
                text.text = msgOG;
                multiple = 1;
            }
        });
        text.text = "";
    }
}
