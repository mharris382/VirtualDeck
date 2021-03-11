using System;
using System.Text;
using UnityEngine;
using static System.String;

[System.Serializable]
public class CardProperty
{
    private const int UNSET_VALUE = Int32.MinValue;
            
    public string element;
    public int intValue = UNSET_VALUE;
    public string textValue;
    public Sprite spriteValue;
    public override string ToString() =>
            $"{element}:" +
            $"{(!IsNullOrEmpty(textValue) ? textValue : "")}" +
            $"{(intValue != UNSET_VALUE ? (object) intValue : null)}";

    public CardProperty Clone()
    {
        return new CardProperty()
        {
             element = element,
             intValue =  intValue,
             textValue = textValue,
             spriteValue = spriteValue
        };
    }
}