
/// <summary>
/// Message Raised when a card has finished being added to the graveyard.
/// </summary>
public class CardWasAddedToGraveyardMessage
{
    public CardInstance card;

    public CardWasAddedToGraveyardMessage(CardInstance card)
    {
        this.card = card;
    }
}