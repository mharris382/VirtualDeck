using System.Collections.Generic;

public class Elements : IElements
{
    public const string Title = "Title";
    public const string Description = "Description";
    public const string AP = "AP";
    public const string Art = "Art";
    public const string CardType = "CardType";


    public string[] elementNames { get; private set; }

    public readonly Dictionary<string, PropertyType> elementTypes;

    private readonly string[] textElementNames = new[] {
        Title,
        Description,
        CardType
    };

    private readonly string[] intElementNames = new[] {
            AP
    };

    private readonly string[] spriteElementNames = new[] {
            Art
    };


    public bool ContainsKey(string propertyName)
    {
        return elementTypes.ContainsKey(propertyName);
    }
    public PropertyType this[string propertyName]
    {
        get => elementTypes[propertyName];
        set => elementTypes.AddOrReplace(propertyName, value);
    }

    public Elements()
    {
        List<string> elts = new List<string>();
        elementTypes  = new Dictionary<string, PropertyType>();
        foreach (var str in spriteElementNames)
        {
            elementTypes.Add(str, PropertyType.Sprite);
            
        }

        foreach (var str in intElementNames)
        {
            elementTypes.Add(str, PropertyType.Int);
        }

        foreach (var str in textElementNames)
        {
            elementTypes.Add(str, PropertyType.Sprite);
            
        }

        this.elementNames = elts.ToArray();
    }

    public void SetProperties(string[] propertyRow)
    {
        this.elementNames = propertyRow;
    }
}

public enum PropertyType
{
    Text,
    Int,
    Sprite
}