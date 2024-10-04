using Meta.Configs.Actions;


namespace Meta.Configs
{
    //вместо энама можно строки разместить
    public  class TypeActionGroup
    {
        public const string Collection = "Collection";

        public const string Inventory  = "Inventory";
        public const string Units = "Units";
        public const string Quests = "Quests";
    }
    
    public interface IActionConfig
    {
        string ActionGroup { get; }
        void Accept(IActionVisitor visitor);
    }

    public interface IActionVisitor
    {
        void Visit(IInventoryAction inventoryActionConfig);
        void Visit (IUnitAction unitActionConfig);
    }
}