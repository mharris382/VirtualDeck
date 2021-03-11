using UnityEngine;

public class CardInstance : MonoBehaviour
{
    private Deck deck;

    private int indexInDeck;
    private bool inited;


    public Card Card => deck.Cards[indexInDeck];

    public void Initialize(Deck deck, int index)
    {
        this.deck = deck;
        this.indexInDeck = index;
        inited = true;
        this.name = Card.Name;
    }
    
    
    
    public class Factory : ICardInstanceFactory
    {
        private Deck deck;

        public Factory(Deck deck)
        {
            this.deck = deck;
        }
    
        public CardInstance CreateCardInstance(int i)
        {
            var instance = new GameObject("Non-inited Card", typeof(CardInstance)).GetComponent<CardInstance>();
            instance.Initialize(deck, i);
            return instance;
        }
    }
}