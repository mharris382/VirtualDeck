using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
 using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public int drawAmount = 5;


    [ChildGameObjectsOnly] [Required]  public Transform handParent;
    [ChildGameObjectsOnly] [Required]  public Transform drawPileParent;
    [ChildGameObjectsOnly] [Required]  public Transform discardPileParent;

    
    public Deck deck;
    public GameManager gm;

    //created by self
    private DrawPile _draw;
    private Hand _hand;
    private DiscardPile _discard;


    private void Awake()
    {
        MessageBroker.Default.Receive<CardAddedToHandMessage>()
                .Where(_ => _hand != null).Subscribe(t =>
                {
                    _hand.Add(t.card);
                });
        MessageBroker.Default.Receive<DiscardMessage>()
                .Where(_ => _discard != null).Subscribe(t =>
                {
                    _discard.Add(t.card);
                });
        
    }

    public void NextTurn()
    {
        MessageBroker.Default.Publish(new NewTurnMessage());
        for (int i = 0; i < drawAmount; i++)
        {
            TryDrawCard();
        }
    }


    public void InitializeDeck(GameManager gameManager)
    {
        void InitPiles()
        {
            _discard = new DiscardPile(discardPileParent);
            _hand = new Hand(handParent, _discard);
            _draw = new DrawPile(drawPileParent, _discard, _hand);
        }
        
        this.gm = gameManager;
        InitPiles();
        deck.InitGameDeck(gm);
        for (int i = 0; i < deck.Cards.Count; i++)
        {
            var instance = new CardInstance.Factory(this.deck).CreateCardInstance(i);
            _discard.Add(instance);
        }
    }

    private event Action @event;

    IEnumerator DoStuff()
    {
        void Foo()
        {
            //DOO STUFF
        }

        @event += Foo;
        yield return new WaitForSeconds(5);
        @event -= Foo;
    }

 
    public void TryDrawCard()
    {
        if (_draw.Count == 0)
        {
            // _draw.AddRange(_discard);
            List<CardInstance> children = new List<CardInstance>(discardPileParent.GetComponentsInChildren<CardInstance>());
            foreach (var child in children)
            {
                child.transform.parent = drawPileParent;
            }
            _draw.Shuffle();
            MessageBroker.Default.Publish(new ReshuffledDeckMessage());
        }

        var c = _draw[0];
        _draw.RemoveAt(0);
        
        
        MessageBroker.Default.Publish(new CardAddedToHandMessage(c));
        
    }


    public void DiscardHand()
    {
        List<CardInstance> cards = new List<CardInstance>();
        if (!_hand.IsNullOrEmpty())
        {
            foreach (var card in _hand)
            {
                var instance = card;
                // _hand.Remove(card);
                cards.Add(instance);
            }
        }
       

        foreach (var cardInstance in cards)
        {
            _hand.Remove(cardInstance);
            MessageBroker.Default.Publish(new DiscardMessage(cardInstance));
        }
    }
    


    
}


