using UnityEngine;

public abstract class CardContainer : TransformAsList<CardInstance>
{
    protected CardContainer(Transform transform1) : base(transform1)
    {
    }
}