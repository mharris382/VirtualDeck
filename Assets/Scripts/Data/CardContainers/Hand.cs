using System.Collections.Generic;
using UnityEngine;

public class Hand : CardContainer
{
    private readonly IList<CardInstance> discardPile;

    public Hand(Transform transform1, IList<CardInstance> discardPile) : base(transform1)
    {
        this.discardPile = discardPile;
    }

       
}