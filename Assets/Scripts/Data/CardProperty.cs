using System.Text;
using UnityEngine;

[System.Serializable]
public class CardProperty
{
    public string element;
    public int intValue = int.MinValue;
    public string textValue;
    public Sprite spriteValue;


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(element);
        sb.Append(':');
        if (!string.IsNullOrEmpty(textValue))
        {
            sb.Append(textValue);
        }
        else if (intValue != int.MinValue)
        {
            sb.Append(intValue);
        }
        else
        {
            sb.Append("Not dealing with sprites");
        }

        return sb.ToString();
    }

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