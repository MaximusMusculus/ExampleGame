namespace Meta.Configs
{
    public enum TypeChange
    {
        AddItem,
        SpendItem,
        AddUnit,
        ChangesArray,
    }

    public abstract class ChangeConfig
    {
        public abstract TypeChange TypeChange { get; }
    }
}