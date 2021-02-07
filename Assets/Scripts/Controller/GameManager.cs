using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Excel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    [FilePath(AbsolutePath = true, Extensions = "xlsx")]
    public string cardLibraryPath;

    
    
    
    private Elements elements = new Elements();



    public Card[] cards;

    public UIManager ui;

    public bool loadOnStart;

    
    
    
    Dictionary<string, Card> cardTable = new Dictionary<string, Card>();


    public DeckManager Deck;

    public void StartGame()
    {
        if(loadOnStart)
            LoadCardsFromXml();

        foreach (var card in cards)
        {
            card.Init(); 
            cardTable.Add(card.name, card);
        }
        
        Deck.InitializeDeck(this);
        
    }

    [Button(ButtonSizes.Gigantic)]
    public void LoadCardsFromXml()
    {
        this.cards = new ExcelDatabaseParser(cardLibraryPath).ParseCardDatabase();
        this.Deck.deck.LoadDeckFromExcel();
    }


    public Card CreateCopyOf(string cardName)
    {
        return cardTable[cardName].Clone();
    }

}