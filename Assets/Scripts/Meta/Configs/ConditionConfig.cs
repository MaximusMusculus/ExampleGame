namespace Meta.Configs
{
    public enum TypeCondition
    {
        None,
        InventoryItemsCount,
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