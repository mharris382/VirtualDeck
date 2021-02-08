using System.Collections.Generic;

public class Elements
{
    public const string Title = "Title";
    public const string Description = "Description";
    public const string AP = "AP";
    public const string Art = "Art";
    
    
    public string[] elementNames = new[]
    {
            Title,
            Description,
            AP,
            Art
    };
    
    
    public enum PropertyType
    {
        Text,
        Int,
        Sprite
    }

  

    public Dictionary<string, PropertyType> elementTypes;


    public Elements()
    {
        elementTypes  = new Dictionary<string, PropertyType>();
        elementTypes.Add(Title, PropertyType.Text);
        elementTypes.Add(AP, PropertyType.Int);
        elementTypes.Add(Description, PropertyType.Text);
        elementTypes.Add(Art, PropertyType.Sprite);
    }
}