using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public TMPro.TextMeshProUGUI titleText;
    public TMPro.TextMeshProUGUI descriptionText;
    public TMPro.TextMeshProUGUI apText;

    public Button discardButton;
    
    private CardInstance displayingCard;
    public Card Card => displayingCard.Card;

    public CardInstance Instance => displayingCard;
    public void ShowCard(CardInstance cardInstance)
    {
        this.displayingCard = cardInstance;
        var card = cardInstance.Card;
        titleText.text = card.name;
        descriptionText.text = card.description;
        apText.text = card.ap.ToString();
    }


    private void Awake()
    {
        if (discardButton == null)
        {
            Debug.LogError("No Discard Button On Card");
            return;
        }
        discardButton.onClick.AddListener(() =>
        {
            MessageBroker.Default.Publish(new CardPlayedMessage(displayingCard));
            MessageBroker.Default.Publish(new DiscardMessage(displayingCard));
        });
    }
    
    
}