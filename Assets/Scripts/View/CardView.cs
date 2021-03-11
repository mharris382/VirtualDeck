﻿using System;
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
        [Required, ChildGameObjectsOnly] public Button destroyButton;
        private CardInstance displayingCard;
        public Card Card => displayingCard.Card;

        public CardInstance Instance => displayingCard;
        
        public void ShowCard(CardInstance cardInstance)
        {
            this.displayingCard = cardInstance;
            var card = cardInstance.Card;
            titleText.text = card.Name;
            descriptionText.text = card.Description;
            apText.text = card.APCost.ToString();
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

            
            if (discardButton)
            {
                discardButton.onClick.AddListener(() =>
                {
                    MessageBroker.Default.Publish(new CardPlayedMessage(displayingCard));
                    MessageBroker.Default.Publish(new AddCardToDiscardPileMessage(displayingCard));
                });
            }
            if (destroyButton)
            {
                destroyButton.onClick.AddListener(() =>
                {
                    MessageBroker.Default.Publish(new CardPlayedMessage(displayingCard));
                    MessageBroker.Default.Publish(new AddCardToGraveyardMessage(displayingCard));
                });
            }

            
            
        }

        private void OnApplicationQuit()
        {
            if (destroyButton)
            {
                destroyButton.onClick.RemoveAllListeners();
            }

            if (discardButton)
            {
                discardButton.onClick.RemoveAllListeners();    
            }
        }
    }
}