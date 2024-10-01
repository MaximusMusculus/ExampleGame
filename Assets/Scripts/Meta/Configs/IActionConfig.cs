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
    }
}