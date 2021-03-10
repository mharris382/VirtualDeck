public interface IElements
{
    string[] elementNames { get; }
    bool ContainsKey(string propertyName);
    PropertyType this[string propertyName] { get; set; }
}