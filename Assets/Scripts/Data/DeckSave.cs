using System;
using System.Collections.Generic;

[Serializable]
public class DeckSave
{
    public int player;
    public int total;
    public DeckSaveCard[] saveCards;

    public DeckSave(Card[] cards) : this(0, cards) { }
    
    public DeckSave(int p, Card[] cards)
    {
        this.player = p;
        Dictionary<string, int> cardCount = new Dictionary<string, int>();
        foreach (var card in cards)
        {
            cardCount.AddIfNotExists(card.Name, 0);
            cardCount[card.Name]++;
        }
        List<DeckSaveCard> save = new List<DeckSaveCard>();
        foreach (var kvp in cardCount)
        {
            save.Add(new DeckSaveCard()
            {
                    cardName = kvp.Key,
                    countInDeck = kvp.Value
            });
        }

        saveCards = save.ToArray();
        total = cards.Length;
    }

    public DeckSave(DeckSaveCard[] saveCards, int p = 0)
    {
        this.saveCards = saveCards;
        this.player = p;
        total = 0;
        for (int i = 0; i < saveCards.Length; i++)
        {
            total += saveCards[i].countInDeck;
        }
    }



    //TODO: test me
    public Card[] BuildDeck(ICardFactory factory)
    {
        Card[] res = new Card[total];

        int next = 0;
        for (int i = 0; i < saveCards.Length; i++)
        {
            var save = saveCards[i];
            for (int j = 0; j < save.countInDeck; j++)
            {
                res[next] = factory.CreateCard(save.cardName);
                next++;
            }
        }
        return res;
    }
}