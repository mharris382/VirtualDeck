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


    [ChildGameObjectsOnly] [Required] public Transform handParent;
    [ChildGameObjectsOnly] [Required] public Transform drawPileParent;
    [ChildGameObjectsOnly] [Required] public Transform discardPileParent;
    [ChildGameObjectsOnly] [Required] public Transform graveyardParent;
    
    public Deck deck;
    public GameManager gm;

    
    private DrawPile _draw;          //created by self in InitPiles()
    private Hand _hand;              //created by self in InitPiles()
    private DiscardPile _discard;    //created by self in InitPiles()
    private Graveyard _graveyard;    //created by self in InitPiles()
    

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
        void InitCardInstances()
        {
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                var instance = new CardInstance.Factory(this.deck).CreateCardInstance(i);
                _draw.Add(instance);
            }
        }

        void InitPiles()
        {
            _discard = new DiscardPile(discardPileParent);
            _hand = new Hand(handParent, _discard);
            _draw = new DrawPile(drawPileParent, _discard, _hand);
            _graveyard = new Graveyard(graveyardParent);

            MessageBroker.Default.Receive<AddCardToHandMessage>().TakeUntilDestroy(this).Subscribe(t => { _hand.Add(t.card); });
            MessageBroker.Default.Receive<AddCardToDiscardPileMessage>().TakeUntilDestroy(this).Subscribe(t => { _discard.Add(t.card); });
            MessageBroker.Default.Receive<AddCardToGraveyardMessage>().TakeUntilDestroy(this).Subscribe(t => { _graveyard.Add(t.card); });
        }
        
        this.gm = gameManager;
        InitPiles();
        InitCardInstances();
    }
    

    public void TryDrawCard()
    {
        if (_draw.Count == 0)
        {
            // _draw.AddRange(_discard);
            List<CardInstance> children = new List<CardInstance>(discardPileParent.GetComponentsInChildren<CardInstance>());
            foreach (var child in children) {
                child.transform.parent = drawPileParent;
            }
            _draw.Shuffle();
            MessageBroker.Default.Publish(new ReshuffledDeckMessage());
        }

        var c = _draw[0];
        _draw.RemoveAt(0);
        
        
        MessageBroker.Default.Publish(new AddCardToHandMessage(c));
        
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
            MessageBroker.Default.Publish(new AddCardToDiscardPileMessage(cardInstance));
        }
    }
    
    
}




