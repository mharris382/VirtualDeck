using System.Collections.Generic;
using UnityEngine;

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