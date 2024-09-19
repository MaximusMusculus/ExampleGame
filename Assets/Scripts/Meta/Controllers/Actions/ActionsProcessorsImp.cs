using System.Collections.Generic;
using System.Linq;
using Meta.Configs;
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
            var targetUnit =_unitController.GetUnits().First(s => s.UnitType == config.TypeUnit);// && s.Stats.Level == args.Progression.Level);
            _unitController.Spend(targetUnit, config.Count);
        }
    }
    
    
    
    /// <summary>
    /// Большой плюс - не надо создавать кучу мелких классов. Нужен только конфиг.
    /// Из минусов, если у процессора изменится тип IActionArgs, то узнаю я это на этапе выполнения (проверку - ожидания/реальность или тесты всех экшенов)
    /// Пока сущонстей немного, можно передавать через конструктор.
    /// Как станет оч много - разбить на специализированные процессоры - введя цепочку обработки с выбросом эксепшена в конце цепочки, если не был экшен обработан
    /// </summary>
    public class ActionProcessor : IActionProcessor
    {
        private readonly Dictionary<TypeAction, IActionProcessor> _actions = new Dictionary<TypeAction, IActionProcessor>();

        public ActionProcessor(IInventoryController inventoryController, IUnitsController unitController)
        {
            _actions.Add(TypeAction.Collection, new ActionCollectionProcessor(this));
            
            _actions.Add(TypeAction.InventoryItemAdd, new InventoryItemAddAction(inventoryController));
            _actions.Add(TypeAction.InventoryItemSpend, new InventoryItemSpendAction(inventoryController));
            _actions.Add(TypeAction.InventoryItemExpandLimit, new InventoryItemExpandLimitAction(inventoryController));
            
            _actions.Add(TypeAction.UnitAdd, new UnitAddAction(unitController));
            _actions.Add(TypeAction.UnitSpend, new UnitSpendAction(unitController));
        }
        
        //как улучшение 
        //private bool CanProcess(TypeAction action);

        public void Process(IActionConfig config)
        {
            _actions[config.TypeAction].Process(config);
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
            foreach (var anyAction in args)
            {
                _actionProcessor.Process(anyAction);
            }
        }
    }
}