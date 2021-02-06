﻿using System;
using System.Collections.Generic;
using System.Linq;
using Excel;
using UnityEngine;

namespace Controller
{
    public interface ICardDatabaseParser
    {
        Card[] ParseCardDatabase();
    }

    public class ExcelDatabaseParser : ICardDatabaseParser
    {
        public string cardLibraryPath;
        private Elements elements = new Elements();
        
        
        public ExcelDatabaseParser(string cardLibraryPath)
        {
            this.cardLibraryPath = cardLibraryPath;
        }



        public Card[] ParseCardDatabase()
        {
            string xmlpath = cardLibraryPath;
            var ws = Workbook.Worksheets(xmlpath).First();
            string[] propertyRow = ws.Rows[0].Cells.Select(t => t.Text).ToArray();
            
            List<Card> cerds = new List<Card>();
            for (int i = 1; i < ws.Rows.Length; i++)
            {
                var strs = ws.Rows[i].Cells.Select(t => t.Text).ToArray();
                cerds.Add( new CardPropertyParser(strs, elements).ParseCard());
            }

            return cerds.ToArray();
        }
        
        private class CardPropertyParser
        {
            private readonly string[] properties;
            private readonly Elements elements;
            public CardPropertyParser(string[] properties, Elements elements)
            {
                this.properties = properties;
                this.elements = elements;
            
            }


            public Card ParseCard()
            {
                var cardProperties = new CardProperty[properties.Length];
                for (int i = 0; i < cardProperties.Length; i++)
                {
                    cardProperties[i] = new CardProperty(){ element = elements.elementNames[i]};
                    ReadProperty(elements, properties[i], ref cardProperties[i]);
                }
                return  new Card(cardProperties.ToArray());
            }

            private void ReadProperty(Elements elements , string raw, ref CardProperty property)
            {
                string elementName = property.element;
                if (elements.elementTypes.ContainsKey(elementName) == false)
                {
                    
                }
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
    
    
    


}