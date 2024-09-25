namespace Meta.Configs
{
    public enum TypeMetaAction
    {
        None,
        Collection,
        
        InventoryItemAdd,           //ItemActionConfig
        InventoryItemSpend,         //ItemActionConfig
        InventoryItemExpandLimit,   //ItemActionConfig

        UnitAdd,        //UnitActionConfig
        UnitSpend,      //UnitActionConfig
        
        
        //допустим
        EventStart, //targetEventId?
        EventStop,  //targetEventId?
    }

    public interface IActionConfig
    {
        TypeMetaAction TypeMetaAction { get; }
    }
}