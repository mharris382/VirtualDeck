using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using View;

public class UIManager : MonoBehaviour
{
    [Required]
    [AssetsOnly]
    public CardView prefab;
    
    [Required]
    public Transform handParent;

    public List<CardView> currentHand;

    private IDisposable disposable;
    private Dictionary<CardInstance, CardView> viewTables = new Dictionary<CardInstance, CardView>();


    private void Awake()
    {
        void OnMoveToDiscard(AddCardToDiscardPileMessage d)
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
                    if (cardView.Card.Name == card.Name)
                    {
                        Destroy(cardView.gameObject);
                    }
                }
            }
        }

        void OnMoveCardToHand(AddCardToHandMessage c)
        {
            var card = c.card;
            var view = GetView(c.card);

            if (viewTables.ContainsKey(card))
                RemoveCardView(viewTables[card]);

            viewTables.AddOrReplace(card, view);
        }

        var cp = new CompositeDisposable();
        MessageBroker.Default.Receive<AddCardToHandMessage>().Subscribe(OnMoveCardToHand).AddTo(cp);
        MessageBroker.Default.Receive<AddCardToDiscardPileMessage>().Subscribe(OnMoveToDiscard).AddTo(cp);
        this.disposable = cp;
    }

    private void OnDestroy() => this.disposable?.Dispose();

    private void OnApplicationQuit() => this.disposable?.Dispose();

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