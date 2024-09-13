namespace Meta.Configs
{
    public enum TypeChange
    {
        AddItem,
        SpendItem,
        AddUnit,
        ChangesArray,
    }

    //или дата?
    public abstract class ChangeConfig
    {
        public abstract TypeChange TypeChange { get; }
    }
}