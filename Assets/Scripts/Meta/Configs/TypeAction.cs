using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Controllers;
using Meta.Models;

namespace Meta.Configs.Action
{
    public enum TypeAction
    {
        InventoryItemAdd,
        InventoryItemSpend,
        InventoryItemExpandLimit,
        
        UnitAdd,
        UnitSpend,
    }
    
    public interface IActionArgs
    {
        TypeAction TypeAction { get; }
    }
    
    public class ItemActionArgs : IActionArgs
    {
        public TypeAction TypeAction => Action;
        
        public TypeAction Action; //availableList?
        public Id TypeItem;
        public int Count;
    }
    
    //ActionConfig?
    public class UnitActionArgs : IActionArgs
    {
        public TypeAction TypeAction => Action;//TypeAction.UnitAdd; // если требуется специализированный параметр
        
        public TypeAction Action;
        public Id TypeUnit;
        public UnitProgressionDto Progression;
        public int Count;
    }
    
    
    public interface IActionProcessor
    {
        void Process(IActionArgs args);
    }
    
    public abstract class ActionAbstract<TArgs> : IActionProcessor where TArgs : IActionArgs, new()
    {
        public void Process(IActionArgs args)
        {
            Process((TArgs) args);
        }
        protected abstract void Process(TArgs args);
        
        public static TArgs CreateEmptyArgs()
        {
            return new TArgs();
        }
    }
    
    public class InventoryItemAddAction: ActionAbstract<ItemActionArgs>
    {
        private readonly IInventoryController _inventoryController;

        public InventoryItemAddAction(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }
        protected override void Process(ItemActionArgs args)
        {
            _inventoryController.Add(args.TypeItem, args.Count);
        }
    }
    public class InventoryItemSpendAction: ActionAbstract<ItemActionArgs>
    {
        private readonly IInventoryController _inventoryController;

        public InventoryItemSpendAction(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }
        protected override void Process(ItemActionArgs args)
        {
            _inventoryController.Spend(args.TypeItem, args.Count);
        }
    }
    public class InventoryItemExpandLimitAction: ActionAbstract<ItemActionArgs>
    {
        private readonly IInventoryController _inventoryController;

        public InventoryItemExpandLimitAction(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }
        protected override void Process(ItemActionArgs args)
        {
            _inventoryController.ExpandLimit(args.TypeItem, args.Count);
        }
    }
    
    public class UnitAddAction: ActionAbstract<UnitActionArgs>
    {
        private readonly IUnitsController _unitController;

        public UnitAddAction(IUnitsController unitController)
        {
            _unitController = unitController;
        }
        protected override void Process(UnitActionArgs args)
        {
            _unitController.Add(args.TypeUnit, args.Progression, args.Count);
        }
    }
    public class UnitSpendAction: ActionAbstract<UnitActionArgs>
    {
        private readonly IUnitsController _unitController;

        public UnitSpendAction(IUnitsController unitController)
        {
            _unitController = unitController;
        }
        protected override void Process(UnitActionArgs args)
        {
            var targetUnit =_unitController.GetUnits().First(s => s.UnitType == args.TypeUnit);// && s.Stats.Level == args.Progression.Level);
            _unitController.Spend(targetUnit, args.Count);
        }
    }
    
    
    public class ActionProcessor : IActionProcessor
    {
        private readonly Dictionary<TypeAction, IActionProcessor> _actions = new Dictionary<TypeAction, IActionProcessor>();

        public ActionProcessor(IInventoryController inventoryController, IUnitsController unitController)
        {
            _actions.Add(TypeAction.InventoryItemAdd, new InventoryItemAddAction(inventoryController));
            _actions.Add(TypeAction.InventoryItemSpend, new InventoryItemSpendAction(inventoryController));
            _actions.Add(TypeAction.InventoryItemExpandLimit, new InventoryItemExpandLimitAction(inventoryController));
            
            _actions.Add(TypeAction.UnitAdd, new UnitAddAction(unitController));
            _actions.Add(TypeAction.UnitSpend, new UnitSpendAction(unitController));
        }

        public void Process(IActionArgs args)
        {
            _actions[args.TypeAction].Process(args);
        }
    }
    
    public class TestAction
    {
        private IActionProcessor _actionProcessor;
        public void Test()
        {

            var itemAction = new ItemActionArgs
            {
                Action = TypeAction.InventoryItemAdd,
                TypeItem = new Id(1),
                Count = 10
            };
            _actionProcessor.Process(itemAction);
            
            var unitAction = new UnitActionArgs
            {
                Action = TypeAction.UnitAdd,
                TypeUnit = new Id(1),
                Progression = new UnitProgressionDto(),
                Count = 10
            };
            
            _actionProcessor.Process(unitAction);
        }
    }
    

}