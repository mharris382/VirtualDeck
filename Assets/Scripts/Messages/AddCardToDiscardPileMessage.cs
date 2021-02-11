public class  AddCardToDiscardPileMessage
{
    public CardInstance card;

    public AddCardToDiscardPileMessage(CardInstance card)
    {
        this.card = card;
    }
}