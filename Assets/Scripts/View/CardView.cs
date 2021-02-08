using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class CardView : MonoBehaviour
    {
        [Required, ChildGameObjectsOnly]  public TMPro.TextMeshProUGUI titleText;
        [Required, ChildGameObjectsOnly]  public TMPro.TextMeshProUGUI descriptionText;
        [Required, ChildGameObjectsOnly]  public TMPro.TextMeshProUGUI apText;
        [Required, ChildGameObjectsOnly]  public UnityEngine.UI.Image art;
        [Required, ChildGameObjectsOnly]  public Button discardButton;
    
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
            try
            {
                art.sprite = card.Art;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
}