namespace Meta.Configs
{
    public enum TypeCondition
    {
        None,
        InventoryItemsCount = 10,  //CountConditionConfig
        InventoryItemsLimit,       //CountConditionConfig
        
        UnitsCount,                //CountConditionConfig
        UnitsLevel,//?
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