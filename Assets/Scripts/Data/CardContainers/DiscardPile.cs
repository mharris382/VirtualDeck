using UnityEngine;

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