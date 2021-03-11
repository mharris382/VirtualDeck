using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Card : IEquatable<Card>
{
    //Do we really need both? if so which to keep?
    private CardProperty[] properties;
    private Dictionary<string, CardProperty> elementKeyToPropDict;

    public string Name => elementKeyToPropDict[Elements.Title].textValue;

    public string Description => elementKeyToPropDict[Elements.Description].textValue;

    public int APCost => elementKeyToPropDict[Elements.AP].intValue;

    public Sprite Art => elementKeyToPropDict[Elements.Art].spriteValue; 
    
    public Card(CardProperty[] properties)
    {
        this.properties = properties;
        
        CreatePropertyDictionary();

        AddAnyMissingRequiredElements();
    }

    private void AddAnyMissingRequiredElements()
    {
        AddMissingStringElement(Elements.Title, "Unnamed Card");
        AddMissingStringElement(Elements.Description, "No Description");
        AddMissingNumberElement(Elements.AP, -1);

        void AddMissingNumberElement(string element, int value)
        {
            if (elementKeyToPropDict.ContainsKey(element) == false)
            {
                elementKeyToPropDict.Add(element, new CardProperty
                {
                        element = Elements.Title,
                        intValue = value
                });
            }
        }

        void AddMissingStringElement(string element, string value)
        {
            if (elementKeyToPropDict.ContainsKey(element) == false)
            {
                elementKeyToPropDict.Add(element, new CardProperty
                {
                        element = Elements.Title,
                        textValue = value
                });
            }
        }
    }

    public void CreatePropertyDictionary()
    {
        elementKeyToPropDict ??= new Dictionary<string, CardProperty>();
        foreach (var property in properties) elementKeyToPropDict.AddOrReplace(property.element, property);
    }

    public Card Clone()
    {
        int size = properties.Length;
        var arr = new CardProperty[size];
        Array.Copy(properties, arr, size);
        return new Card(arr);
    }


    public override string ToString() => properties.Aggregate("", (current, cardProperty) => current + cardProperty);

    public bool Equals(Card other) => other != null && Equals(Name, other.Name);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((Card) obj);
    }

    public override int GetHashCode() => Name.GetHashCode();
}