using System;
using System.Collections.Generic;

[System.Serializable]
public class Card : IEquatable<Card>
{
    public CardProperty[] _properties;
    public Dictionary<string, CardProperty> propTable;

    public string name => propTable[Elements.Title].textValue;

    public string description => propTable[Elements.Description].textValue;

    public int ap => propTable[Elements.AP].intValue;
    
    
    
    public Card(CardProperty[] properties)
    {
        this._properties = properties;
        this.propTable = new Dictionary<string, CardProperty>();
        foreach (var cardProperty in properties)
        {
            this.propTable.Add(cardProperty.element, cardProperty);
        }

        AddMissingElement(Elements.Title, "Unnamed Card");
        AddMissingElement(Elements.Description, "No Description");
        AddMissingElement(Elements.AP, -1);
        
    }

    public void Init()
    {
        propTable = new Dictionary<string, CardProperty>();
        foreach (var property in _properties)
        {
            propTable.AddOrReplace(property.element, property);
        }
    }

    public Card Clone()
    {
        int size = _properties.Length;
        var arr = new CardProperty[size];
        Array.Copy(this._properties, arr, size);
        return new Card(arr);
    }


    private void AddMissingElement(string element, int value)
    {
        if (propTable.ContainsKey(element) == false)
        {
            propTable.Add(element, new CardProperty()
            {
                    element = Elements.Title,
                    intValue = value
            });
        }
    }
    
    private void AddMissingElement(string element, string value)
    {
        if (propTable.ContainsKey(element) == false)
        {
            propTable.Add(element, new CardProperty()
            {
                    element = Elements.Title,
                    textValue = value
            });
        }
    }

    public override string ToString()
    {
        string s = "";
        foreach (var cardProperty in _properties)
        {
            s += cardProperty.ToString();
            
        }

        return s;
        
    }

    public bool Equals(Card other)
    {
        if (other == null) return false;
        return Equals(this.name, other.name);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Card) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return name.GetHashCode();
        }
    }
}