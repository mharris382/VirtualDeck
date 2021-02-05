using System;
using System.Collections.Generic;

[Serializable]
public class DeckSave
{
    public int player;
    public DeckSaveCard[] saveCards;
    
    [Serializable]
    public struct CardInDeck
    {
        public string cardName;
        public int copies;
    }



    public DeckSave(int p, Card[] cards)
    {
        this.player = p;
        Dictionary<string, int> cardCount = new Dictionary<string, int>();
        foreach (var card in cards)
        {
            cardCount.AddIfNotExists(card.name, 0);
            cardCount[card.name]++;
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

    }
}