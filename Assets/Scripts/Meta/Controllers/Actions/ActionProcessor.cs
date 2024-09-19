using System;
using System.Collections.Generic;
using Meta.Configs;

namespace Meta.Controllers.Actions
{
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
            if (_actions.TryGetValue(config.TypeAction, out var action))
            {
                action.Process(config);
            }
            throw new ArgumentException($"Action {config.TypeAction} not found");
        }
    }
}