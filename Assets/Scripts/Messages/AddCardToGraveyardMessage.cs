/// <summary>
/// Message to call when you want to send a card to the graveyard
/// </summary>
public class AddCardToGraveyardMessage
{
    public CardInstance card;

    public AddCardToGraveyardMessage(CardInstance card)
    {
        this.card = card;
    }
}