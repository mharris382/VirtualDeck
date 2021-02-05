[System.Serializable]
public struct DeckSaveCard 
{
    public string cardName;
    public int countInDeck;

    public DeckSaveCard(string cardName, int countInDeck)
    {
        this.cardName = cardName;
        this.countInDeck = countInDeck;
    }
    
}