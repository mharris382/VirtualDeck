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
    public Deck deck;
    public GameManager gm;

    public int drawAmount = 5;
    
    
 [ChildGameObjectsOnly] [Required]  public Transform handParent;
 [ChildGameObjectsOnly] [Required]  public Transform drawPileParent;
 [ChildGameObjectsOnly] [Required]  public Transform discardPileParent;


    private DrawPile _draw;
    private Hand _hand;
    private DiscardPile _discard;


    [HideInEditorMode]
    [Button()]
    public void NextTurn()
    {
        MessageBroker.Default.Publish(new NewTurnMessage());
        for (int i = 0; i < drawAmount; i++)
        {
            TryDrawCard();
        }
    }
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

    public void InitPiles()
    {
        _discard = new DiscardPile(discardPileParent);
        _hand = new Hand(handParent, _discard);
        _draw = new DrawPile(drawPileParent, _discard, _hand);
    }

    public void InitializeDeck(GameManager gameManager)
    {
        this.gm = gameManager;
        InitPiles();
        deck.InitGameDeck(gm);
        for (int i = 0; i < deck.Cards.Count; i++)
        {
            var instance = CreateInstanceOfCardAtIndex(i);
            _discard.Add(instance);
        }
    }

    [HideInEditorMode]
    [Button]
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

    [HideInEditorMode]
    [Button]
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
    
    
    #region [Containers]

    public class DrawPile : CardContainer
    {
        private readonly IList<CardInstance> discardPile;
        private readonly IList<CardInstance> hand;

        public DrawPile(Transform transform1, IList<CardInstance> discardPile , IList<CardInstance> hand) : base(transform1)
        {
            this.discardPile = discardPile;
            this.hand = hand;
        }


        public void Shuffle()
        {

            for (int i = Count - 1; i >= 0; i--)
            {
                var indexRandom = UnityEngine.Random.Range(0, i);
                var temp = this[i];
                int prevIndex = temp.transform.GetSiblingIndex();
                var prevCard = this[indexRandom];
                temp.transform.SetSiblingIndex(indexRandom);
                prevCard.transform.SetSiblingIndex(prevIndex);
                
            }
        }

        public override void Add(CardInstance item)
        {
            base.Add(item);
        }
    }
    
    public class DiscardPile : CardContainer
    {
        public DiscardPile(Transform transform1) : base(transform1)
        {
        }

        public override bool Remove(CardInstance item)
        {
            Debug.Log($"Removed from the Discard {item.Card.name} Pile" );
            return true;
        }
    }
    
    public class Hand : CardContainer
    {
        private readonly IList<CardInstance> discardPile;

        public Hand(Transform transform1, IList<CardInstance> discardPile) : base(transform1)
        {
            this.discardPile = discardPile;
        }

       
    }

    public abstract class CardContainer : TransformAsList<CardInstance>
    {
        protected CardContainer(Transform transform1) : base(transform1)
        {
        }
    }

    #endregion

    private CardInstance CreateInstanceOfCardAtIndex(int i)
    {
        var instance = new GameObject("Non-inited Card", typeof(CardInstance)).GetComponent<CardInstance>();
        instance.InitializeInstance(deck, i);
        return instance;
    }
}

public enum DeckLocation
{
    DrawPile = 0,
    DiscardPile = 1,
    Hand = 2,
    Graveyard = 3,
    Equipped = 4
}