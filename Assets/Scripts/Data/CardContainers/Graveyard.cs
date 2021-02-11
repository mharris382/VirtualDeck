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