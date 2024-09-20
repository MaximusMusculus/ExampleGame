namespace Meta.Configs
{
    public enum TypeAction
    {
        None,
        Collection,
        
        InventoryItemAdd,
        InventoryItemSpend,
        InventoryItemExpandLimit,

        UnitAdd,
        UnitSpend,
        
        
        //допустим
        EventStart,
        EventStop,
    }

    public interface IActionConfig
    {
        TypeAction TypeAction { get; }
    }
}