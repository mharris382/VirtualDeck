using System;
using System.Collections.Generic;
using System.Linq;
using Excel;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [FilePath(AbsolutePath = true, Extensions = "xlsx")]
    public string cardLibraryPath;
    [FilePath(AbsolutePath = true, Extensions = "txt")]
    public string deckFilePath;
   
    [FilePath(AbsolutePath = false, Extensions = "json,txt")]
    public string deckFileOutputPath;
    
    
    
    private Elements elements = new Elements();



    public Card[] cards;

    public UIManager ui;

    public bool loadOnStart;

    
    
    
    Dictionary<string, Card> cardTable = new Dictionary<string, Card>();


    public DeckManager Deck;


    private void Start()
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
        
        string xmlpath = cardLibraryPath;
        var ws = Workbook.Worksheets(xmlpath).First();
        string[] propertyRow = ws.Rows[0].Cells.Select(t => t.Text).ToArray();
        List<Card> cerds = new List<Card>();
        for (int i = 1; i < ws.Rows.Length; i++)
        {
            var strs = ws.Rows[i].    Cells.Select(t => t.Text).ToArray();
            var reader = new PropertyReader(strs, new Elements());
            
            Debug.Log(reader.Card.ToString());
            cerds.Add(reader.Card);
        }

        this.cards = cerds.ToArray();
        
        this.Deck.deck.LoadDeckFromExcel();
    }


    public Card CreateCopyOf(string cardName)
    {
        return cardTable[cardName].Clone();
    }


    public class PropertyReader
    {
        public Card Card;
        public PropertyReader(string[] properties, Elements elements)
        {
            var cardProperties = new CardProperty[properties.Length];
            for (int i = 0; i < cardProperties.Length; i++)
            {
                cardProperties[i] = new CardProperty(){ element = elements.elementNames[i]};
                ReadProperty(elements, properties[i], ref cardProperties[i]);
            }
            Card = new Card(cardProperties.ToArray());
        }

        private void ReadProperty(Elements elements , string raw, ref CardProperty property)
        
        {
            string elementName = property.element;
            var type = elements.elementTypes[elementName];
            switch (type)
            {
                case Elements.PropertyType.Text:
                    property.textValue = raw;
                    break;
                case Elements.PropertyType.Int:
                    property.intValue = Int32.Parse(raw);
                    break;
                case Elements.PropertyType.Sprite:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        
    }
}