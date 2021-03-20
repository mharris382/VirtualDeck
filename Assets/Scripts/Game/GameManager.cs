using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Excel;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DeckManager Deck;
    
    [Tooltip("If enabled the card library will be parsed from excel before the game is started")]
    public bool loadOnStart;

    [Tooltip("Absolute path to the excel file containing card data.  All cards should be on the first sheet, with the 1st row containing headers")]
    [FilePath(AbsolutePath = true, Extensions = "xlsx")]
    public string cardLibraryPath;


    public Card[] cards;
    private Elements elements = new Elements();
    private Dictionary<string, Card> cardTable;






    

    private void Awake()
    {
        cardTable = new Dictionary<string, Card>();
    }

    public void OnApplicationQuit()
    {
        cardTable.Clear();
    }

    public void StartGame()
    {
        
        if(loadOnStart)
            LoadCardsFromXml();

        foreach (var card in cards)
        {
            card.CreatePropertyDictionary(); 
            cardTable.Add(card.Name, card);
        }
        
        Deck.InitializeDeck(this);
        
    }

    
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