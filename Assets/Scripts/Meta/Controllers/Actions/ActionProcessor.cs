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
        private readonly Dictionary<string, IActionProcessor> _actions = new Dictionary<string, IActionProcessor>();

        public ActionProcessor(IInventoryController inventoryController, IUnitsController unitController)
        {
            var inventoryProcessor = new InventoryActionsProcessor(inventoryController); //create from factory?
            var unitsProcessor = new UnitActionProcessor(unitController);
            _actions.Add(TypeActionGroup.Collection, new ActionProcessorCollectionProcessor(this));
            _actions.Add(TypeActionGroup.Inventory, inventoryProcessor);
            _actions.Add(TypeActionGroup.Units, unitsProcessor);
        }
        

        public void Process(IActionConfig actionConfig)
        {
            if (_actions.TryGetValue(actionConfig.ActionGroup, out var action) == false)
            {
                throw new ArgumentException($"Action {actionConfig.ActionGroup} not found");
            }
            action.Process(actionConfig);
        }
    }
}