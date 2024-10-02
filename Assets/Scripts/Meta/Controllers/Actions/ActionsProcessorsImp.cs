using AppRen;
using Meta.Configs.Actions;
using Meta.Models;


namespace Meta.Controllers.Actions
{
    public class UnitActionProcessor : ActionProcessorAbstract<IUnitAction>, IUnitActionVisitor
    {
        private readonly IUnitsController _unitController;

        public UnitActionProcessor(IUnitsController unitController)
        {
            _unitController = unitController;
        }

        protected override void Process(IUnitAction action)
        {
            action.Visit(this);
        }
        
        public void UnitAdd(Id typeUnit, UnitProgressionDto progression, int count)
        {
            _unitController.Add(typeUnit, progression, count);
        }

        public void UnitSpend(Id typeUnit, UnitProgressionDto progression, int count)
        {
            if (_unitController.TryGetUnit(typeUnit, progression, out var unit))
            {
                _unitController.Spend(unit, count);
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

        protected override void Process(IInventoryAction action)
        {
            action.Visit(this);
        }

        public void ItemAdd(Id itemId, int count)
        {
            _inventoryController.Add(itemId, count);
        }
        
        public void ItemSpend(Id itemId, int count)
        {
            _inventoryController.Spend(itemId, count);
        }
        
        public void ItemExpandLimit(Id itemId, int count)
        {
            _inventoryController.ExpandLimit(itemId, count);
        }
    }

    public class ActionCollectionProcessor : ActionProcessorAbstract<IActionCollectionConfig>
    {
        private readonly IActionProcessor _actionProcessor;

        public ActionCollectionProcessor(IActionProcessor actionProcessor)
        {
            _actionProcessor = actionProcessor;
        }

        protected override void Process(IActionCollectionConfig action)
        {
            foreach (var anyAction in action.GetAll())
            {
                _actionProcessor.Process(anyAction);
            }
        }
    }
}