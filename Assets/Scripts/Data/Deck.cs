using System;
using System.Collections.Generic;
using System.Linq;
using Excel;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "New Deck")]
public class Deck : ScriptableObject
{
    [FilePath(Extensions = "xlsx")]
    public string excelPath;


    public int sheet = 1;
    public int column = 1;

    public int cardTitleColumn = 0;
    public DeckSaveCard[] cardsInDeck;


    
    public List<Card> Cards { get; private  set; }

    [Button(ButtonSizes.Gigantic)]
    public void LoadDeckFromExcel()
    {
        cardsInDeck = new ExcelDeckLoader(sheet, column).LoadDeckFromSave(excelPath);
    }

    public void InitGameDeck()
    {
        Cards = new List<Card>();
        
        foreach (var deckSaveCard in cardsInDeck)
        {
            for (int i = 0; i < deckSaveCard.countInDeck; i++)
            {
                throw new NotImplementedException("Create Copy of Saved Card for gameplay");
                //Cards.Add(gm.CreateCopyOf(deckSaveCard.cardName));
            }
        }
    }
    
}


