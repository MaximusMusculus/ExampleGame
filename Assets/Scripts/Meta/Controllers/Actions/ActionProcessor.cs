﻿using System;
using System.Collections.Generic;
using Meta.Configs;
using Meta.Configs.Actions;


namespace Meta.Controllers.Actions
{
    /*
    public class ActionExecuteProcessor : IActionProcessor
    {
        private readonly Dictionary<string, IActionProcessor> _actions = new Dictionary<string, IActionProcessor>(ConstDefaultCapacity.Micro);
        public void AddProcessor(string actionGroup, IActionProcessor processor)
        {
            _actions.Add(actionGroup, processor);
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
    public class ActionQuestProcessor : IActionProcessor
    {
        private readonly Dictionary<string, IActionProcessor> _actions = new Dictionary<string, IActionProcessor>(ConstDefaultCapacity.Micro);
        
        public void AddProcessor(string actionGroup, IActionProcessor processor)
        {
            _actions.Add(actionGroup, processor);
        }
        
        public void Process(IActionConfig actionConfig)
        {
            //Квестам не обязательно обрабатывать все действия. Поэтому тех, что не нудно - пропускаем
            if (_actions.TryGetValue(actionConfig.ActionGroup, out var action))
            {
                action.Process(actionConfig);
            }
        }
    }
    */
    
    
    
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
            _actions.Add(TypeActionGroup.Collection, new ActionCollectionProcessor(this));
            _actions.Add(TypeActionGroup.Inventory, new InventoryActionsProcessor(inventoryController));
            _actions.Add(TypeActionGroup.Units, new UnitActionProcessor(unitController));
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