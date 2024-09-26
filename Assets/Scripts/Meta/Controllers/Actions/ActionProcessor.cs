using System;
using System.Collections.Generic;
using Meta.Configs;
using Meta.Configs.Actions;


namespace Meta.Controllers.Actions
{
    /// <summary>
    /// Возможность проверить - это хорошо, но надо подуматЬ, как этого можно избежать?
    /// Если это из конфига берется, то можно при создании - проконтролировать.
    /// 
    /// </summary>
    public class ActionAssertTypeProcessor : IActionProcessor
    {
        private readonly TypeMetaAction _typeMetaAction;
        private Dictionary<TypeMetaAction, Type> _types;
        private IActionProcessor _nextProcessor;

        public ActionAssertTypeProcessor(IActionProcessor nextProcessor)
        {
            _nextProcessor = nextProcessor;
            _types = new Dictionary<TypeMetaAction, Type>
            {
                {TypeMetaAction.Collection, typeof(ActionCollectionConfig)},
                {TypeMetaAction.InventoryItemAdd, typeof(ItemActionConfig)},
                {TypeMetaAction.InventoryItemSpend, typeof(ItemActionConfig)},
                {TypeMetaAction.InventoryItemExpandLimit, typeof(ItemActionConfig)},
                {TypeMetaAction.UnitAdd, typeof(UnitActionConfig)},
                {TypeMetaAction.UnitSpend, typeof(UnitActionConfig)}
            };
        }

        public void Process(IActionConfig actionConfig)
        {
            if (_types.TryGetValue(actionConfig.TypeMetaAction, out var type))
            {
                if(type.IsInstanceOfType(actionConfig) == false)
                {
                    throw new ArgumentException($"Action {actionConfig.TypeMetaAction} " +
                                                $"not match type {type.Name}. Info:{actionConfig.GetType().Name}:{actionConfig}");
                }
                _nextProcessor.Process(actionConfig);
                return;
            }
            throw new ArgumentException($"Action check: {actionConfig.TypeMetaAction} not found");
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
        private readonly Dictionary<TypeMetaAction, IActionProcessor> _actions = new Dictionary<TypeMetaAction, IActionProcessor>();
        
        public ActionProcessor(IInventoryController inventoryController, IUnitsController unitController)
        {
            _actions.Add(TypeMetaAction.Collection, new ActionCollectionProcessor(this));
            
            _actions.Add(TypeMetaAction.InventoryItemAdd, new InventoryItemAddAction(inventoryController));
            _actions.Add(TypeMetaAction.InventoryItemSpend, new InventoryItemSpendAction(inventoryController));
            _actions.Add(TypeMetaAction.InventoryItemExpandLimit, new InventoryItemExpandLimitAction(inventoryController));
            
            _actions.Add(TypeMetaAction.UnitAdd, new UnitAddAction(unitController));
            _actions.Add(TypeMetaAction.UnitSpend, new UnitSpendAction(unitController));
        }
        
        //как улучшение 
        //private bool CanProcess(TypeAction action);

        public void Process(IActionConfig actionConfig)
        {
            if (_actions.TryGetValue(actionConfig.TypeMetaAction, out var action) == false)
            {
                throw new ArgumentException($"Action {actionConfig.TypeMetaAction} not found");
            }
            action.Process(actionConfig);
        }
    }
}