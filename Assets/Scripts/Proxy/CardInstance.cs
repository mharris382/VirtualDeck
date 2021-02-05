using UnityEngine;

public class CardInstance : MonoBehaviour
{
    private Deck deck;

    private int indexInDeck;
    private bool inited;


    public Card Card => deck.Cards[indexInDeck];

    public void InitializeInstance(Deck deck, int index)
    {
        this.deck = deck;
        this.indexInDeck = index;
        inited = true;
        this.name = Card.name;
    }
}