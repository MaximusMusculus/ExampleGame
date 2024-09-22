using System.Linq;
using Meta.Configs.Actions;


namespace Meta.Controllers.Actions
{
    //Пока в одном файле, потом можно разбить
    
    public class InventoryItemAddAction: ActionAbstract<ItemActionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public InventoryItemAddAction(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }
        protected override void Process(ItemActionConfig config)
        {
            _inventoryController.Add(config.TypeItem, config.Count);
        }
    }
    public class InventoryItemSpendAction: ActionAbstract<ItemActionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public InventoryItemSpendAction(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }
        protected override void Process(ItemActionConfig config)
        {
            _inventoryController.Spend(config.TypeItem, config.Count);
        }
    }
    public class InventoryItemExpandLimitAction: ActionAbstract<ItemActionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public InventoryItemExpandLimitAction(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }
        protected override void Process(ItemActionConfig config)
        {
            _inventoryController.ExpandLimit(config.TypeItem, config.Count);
        }
    }
    
    public class UnitAddAction: ActionAbstract<UnitActionConfig>
    {
        private readonly IUnitsController _unitController;

        public UnitAddAction(IUnitsController unitController)
        {
            _unitController = unitController;
        }
        protected override void Process(UnitActionConfig config)
        {
            _unitController.Add(config.TypeUnit, config.Progression, config.Count);
        }
    }
    public class UnitSpendAction: ActionAbstract<UnitActionConfig>
    {
        private readonly IUnitsController _unitController;

        public UnitSpendAction(IUnitsController unitController)
        {
            _unitController = unitController;
        }
        protected override void Process(UnitActionConfig config)
        {
            if (_unitController.TryGetUnit(config.TypeUnit, config.Progression, out var unit))
            {
                _unitController.Spend(unit, config.Count);
            }
        }
    }


    public class ActionCollectionProcessor : ActionAbstract<ActionCollectionConfig>
    {
        private readonly IActionProcessor _actionProcessor;

        public ActionCollectionProcessor(IActionProcessor actionProcessor)
        {
            _actionProcessor = actionProcessor;
        }

        protected override void Process(ActionCollectionConfig args)
        {
            foreach (var anyAction in args)//.GetActions())
            {
                _actionProcessor.Process(anyAction);
            }
        }
    }
}