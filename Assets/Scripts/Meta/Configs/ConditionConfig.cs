namespace Meta.Configs
{
    public enum TypeCondition
    {
        InventoryItemsCount,
        InventoryItemsLimit,
        UnitsCount,
        //Level,
        //maps
        //...
    }

    public interface IConditionConfig
    {
        TypeCondition TypeCondition { get; }
    }
}