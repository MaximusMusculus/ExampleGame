namespace Meta.Configs
{
    public enum TypeCondition
    {
        None,
        InventoryItemsCount = 10,
        InventoryItemsLimit,
        UnitsCount,
        //Level,
        //maps
        //...
        AndCollection,
        OrCollection,
    }

    public interface IConditionConfig
    {
        TypeCondition TypeCondition { get; }
    }
}