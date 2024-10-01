namespace Meta.Configs
{
    //вместо энама можно строки разместить
    public partial class TypeActionGroup
    {
        public const string Collection = "Collection";
        
        public const string Inventory = "Inventory";
        public const string Units = "Unit";
        //....
        //....
        //...
        //..
        //.
    }
    
    /*public partial class TypeAction
    {
        public static class ActionsInventory
        {
            public const string ItemAdd = "ItemAdd";
            public const string ItemSpend = "ItemSpend";
            public const string ItemExpandLimit = "ItemExpandLimit";
        }
    }
    public partial class TypeAction
    {
        public static class ActionsUnit
        {
            public const string UnitAdd = "UnitAdd";
            public const string UnitSpend = "UnitSpend";
        }
    }*/
    
    
    
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
       // TypeMetaAction TypeMetaAction { get; }  //заменить на строку
        string ActionGroup { get; }
    }
}