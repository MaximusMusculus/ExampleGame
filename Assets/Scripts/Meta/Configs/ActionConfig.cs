namespace Meta.Configs
{
    public enum TypeAction
    {
        InventoryItemAdd,
        InventoryItemSpend,
        InventoryItemExpandLimit,

        UnitAdd,
        UnitSpend,
        Collection,
    }

    public interface IActionConfig
    {
        TypeAction TypeAction { get; }
    }
}