using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Required]
    [AssetsOnly]
    public CardView prefab;
    
    [Required]
    public Transform handParent;

    public List<CardView> currentHand;


    private Dictionary<CardInstance, CardView> viewTables = new Dictionary<CardInstance, CardView>();


    private void Awake()
    {
        MessageBroker.Default.Receive<CardAddedToHandMessage>().Subscribe(c =>
        {
            var card = c.card;
            var view = GetView(c.card);
            
            if (viewTables.ContainsKey(card)) 
                RemoveCardView(viewTables[card]);

            viewTables.AddOrReplace(card, view);
        });

        MessageBroker.Default.Receive<DiscardMessage>().Subscribe(d =>
        {
            Debug.Log("UI Discard Message");
            if (viewTables.ContainsKey(d.card))
            {
                RemoveCardView(viewTables[d.card]);
            }
            else
            {
                Card card = d.card.Card;
                foreach (var cardView in currentHand)
                {
                    if (cardView.Card.name == card.name)
                    {
                        Destroy(cardView.gameObject);
                    }
                }
            }
        });
    }

    private CardView GetView(CardInstance card)
    {
        var instance = Instantiate(prefab, handParent);
        instance.ShowCard(card);
        return instance;
    }

    private void RemoveCardView(CardView view)
    {
        if (view == null) return;
        Destroy(view.gameObject);
    }
    
}