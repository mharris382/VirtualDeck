using System.Collections.Generic;
using System.Linq;
using Excel;
using UnityEngine;

public class ExcelDeckLoader : IDeckLoader
{
    public int sheet = 1;
    public int column = 1;

    public ExcelDeckLoader( int sheet, int column)
    {
        this.sheet = sheet;
        this.column = column;
    }

    public DeckSaveCard[] LoadDeckFromSave(string path)
    {
        string excelPath = path;
        string xmlpath = excelPath;
        var ws = Workbook.Worksheets(xmlpath).ToArray()[sheet];
        int total = 0;
        List<DeckSaveCard> cardSave = new List<DeckSaveCard>();
        foreach (var row in ws.Rows)
        {
            string cardName = row.Cells[0].Text;
            int copies = (int)row.Cells[column].Amount;
            cardSave.Add(new DeckSaveCard()
            {
                    cardName = cardName,
                    countInDeck = copies
            });
            total += copies;
        }
        Debug.Log("Total # of Cards in deck = " + total);
        return cardSave.ToArray();
    }
}