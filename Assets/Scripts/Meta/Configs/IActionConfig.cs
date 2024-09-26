namespace Meta.Configs
{
    //вместо энама можно строки разместить
    public static class TypeAction
    {
        public const string Collection = "Collection";//??
        
        public const string ItemAdd = "ItemAdd";
        public const string ItemSpend = "ItemSpend";
        public const string ItemExpandLimit = "ItemExpandLimit";
        
        public const string UnitAdd = "UnitAdd";
        public const string UnitSpend = "UnitSpend";
        
    }
    
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