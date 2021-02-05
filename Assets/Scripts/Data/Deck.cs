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
        string xmlpath = excelPath;
        var ws = Workbook.Worksheets(xmlpath).ToArray()[sheet];
        int total = 0;
        List<DeckSaveCard> cardSave = new List<DeckSaveCard>();
        foreach (var row in ws.Rows)
        {
            string cardName = row.Cells[cardTitleColumn].Text;
            int copies = (int)row.Cells[column].Amount;
            cardSave.Add(new DeckSaveCard()
            {
                    cardName = cardName,
                    countInDeck = copies
            });
            total += copies;
        }    
        Debug.Log("Total # of Cards in deck = " + total);
        cardsInDeck = cardSave.ToArray();
    }

    public void InitGameDeck(GameManager gm)
    {
        Cards = new List<Card>();
        
        foreach (var deckSaveCard in cardsInDeck)
        {
            for (int i = 0; i < deckSaveCard.countInDeck; i++)
            {
                Cards.Add(gm.CreateCopyOf(deckSaveCard.cardName));
            }
        }
    }
    
}