namespace Meta.Configs
{
    //вместо энама можно строки разместить
    public  class TypeActionGroup
    {
        public static string Collection => "Collection";

        public static string Inventory  => "Inventory";
        public static string Units => "Unit";
    }


    public enum TypeMetaAction
    {
        None,
        Collection,

        InventoryItemAdd, //ItemActionConfig
        InventoryItemSpend, //ItemActionConfig
        InventoryItemExpandLimit, //ItemActionConfig

        UnitAdd, //UnitActionConfig
        UnitSpend, //UnitActionConfig
    }

    public interface IActionConfig
    {
        string ActionGroup { get; }
    }
}