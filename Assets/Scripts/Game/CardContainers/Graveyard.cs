using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Graveyard : CardContainer
{
    public Graveyard(Transform transform1) : base(transform1)
    {
        transform1.name = "Graveyard";
    }

    public override void Add(CardInstance item)
    {
        base.Add(item);
        MessageBroker.Default.Publish(new CardWasAddedToGraveyardMessage(item));
    }
}

public class CardCreationQueueHolder : CardContainer
{
    private readonly IList<CardInstance> hand;


    public CardCreationQueueHolder(Transform transform1, IList<CardInstance> hand) : base(transform1)
    {
        this.hand = hand;
    }
}


public class CreationQueue
{
    private readonly Deck deck;
    private readonly IList<CardInstance> hand;
    private readonly ICardInstanceFactory cardInstanceFactory;

    Queue<CardInstance> creationQueue = new Queue<CardInstance>();
    List<CardInstance> available = new List<CardInstance>();
    List<CardInstance> active = new List<CardInstance>();

    public CreationQueue(IList<CardInstance> hand, Deck deck, ICardInstanceFactory cardInstanceFactory)
    {
        this.hand = hand;
        this.deck = deck;
        this.cardInstanceFactory = cardInstanceFactory;
    }

    public void AddCardToQueue(string cardName)
    {
        CardInstance instance = null;
        foreach (var readyCard in available)
        {
            if (readyCard.Card.name == cardName)
            {
                instance = readyCard;
                break;
            }
        }

        if (instance == null)
        {
            var cardIndex = deck.Cards.FindIndex(t => t.name == cardName);
            
            if (cardIndex == -1)
                throw new NullReferenceException($"No card named {cardName} exists!");
            
            instance = cardInstanceFactory.CreateCardInstance(cardIndex);
        } else {
            available.Remove(instance);
        }

        creationQueue.Enqueue(instance);
    }


    public void OnNewTurn()
    {
        while (creationQueue.Count > 0)
        {
            MessageBroker.Default.Publish(new AddCardToHandMessage(creationQueue.Dequeue()));
        }
    }
}

public enum GameTriggers
{
    TurnStart = 1,
    EnterPlayerPhase = 2,
    PlayerPhase = 3,
    ExitPlayerPhase = 4,
    MinionPhase = 5,
    BossPhase = 6,
    TurnEnd = 7,
}