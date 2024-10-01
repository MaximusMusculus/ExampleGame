using Meta.Configs.Actions;


namespace Meta.Controllers.Actions
{
    public class ActionProcessorCollectionProcessor : ActionProcessorAbstract<ActionCollectionConfig>
    {
        private readonly IActionProcessor _actionProcessor;

        public ActionProcessorCollectionProcessor(IActionProcessor actionProcessor)
        {
            _actionProcessor = actionProcessor;
        }

        protected override void Process(ActionCollectionConfig args)
        {
            foreach (var anyAction in args.GetAll())
            {
                _actionProcessor.Process(anyAction);
            }
        }
    }
    
    public class InventoryActionsProcessor : ActionProcessorAbstract<IInventoryAction>, IInventoryActionVisitor
    {
        private readonly IInventoryController _inventoryController;

        public InventoryActionsProcessor(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override void Process(IInventoryAction args)
        {
            args.Visit(this);
        }

        public void ItemAdd(ItemActionConfig itemActionConfig)
        {
            _inventoryController.Add(itemActionConfig.TypeItem, itemActionConfig.Count);
        }
        public void ItemSpend(ItemActionConfig itemActionConfig)
        {
            _inventoryController.Spend(itemActionConfig.TypeItem, itemActionConfig.Count);
        }
        public void ItemExpandLimit(ItemActionConfig itemActionConfig)
        {
            _inventoryController.ExpandLimit(itemActionConfig.TypeItem, itemActionConfig.Count);
        }
    }
    
    public class UnitActionProcessor : ActionProcessorAbstract<IUnitAction>, IUnitActionVisitor
    {
        private readonly IUnitsController _unitController;

        public UnitActionProcessor(IUnitsController unitController)
        {
            _unitController = unitController;
        }

        protected override void Process(IUnitAction args)
        {
            args.Visit(this);
        }

        public void UnitAdd(UnitActionConfig unitActionConfig)
        {
            _unitController.Add(unitActionConfig.TypeUnit, unitActionConfig.Progression, unitActionConfig.Count);
        }

        public void UnitSpend(UnitActionConfig unitActionConfig)
        {
            if (_unitController.TryGetUnit(unitActionConfig.TypeUnit, unitActionConfig.Progression, out var unit))
            {
                _unitController.Spend(unit, unitActionConfig.Count);
            }
        }
    }
}