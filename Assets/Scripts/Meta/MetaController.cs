using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Conditions;
using Meta.Controllers.Imp;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta
{
    /// <summary>
    /// Model???
    /// </summary>
    public class MetaModel
    {
        public IInventoryController Inventory { get; }
        public IUnitsController Units { get; }
        
        private readonly IActionProcessor _actions;
        private readonly IConditionProcessor _conditions;
        private readonly MetaConfig _config;
        public MetaModel(MetaConfig config, MetaDto data)
        {
            _config = config;
            Inventory = new InventoryController(config.InventoryItems, data.Items);
            Units = new UnitsController(config.Units, data.Units);

            _actions = new ActionProcessor(Inventory, Units);
            _conditions = new ConditionProcessor(Inventory, Units);
        }
        
        public void DoAction(Id group, int index)
        {
            foreach (var actionsGroup in _config.ActionsGroups)
            {
                if (actionsGroup.TypeGroup.Equals(group))
                {
                    var action = actionsGroup.Actions[index];
                    Assert.IsTrue(_conditions.Check(action.Require));
                    _actions.Process(action.Actions);
                }
            }
        }

        
        /*public void RunAction(MetaActionConfig actionConfig)
        {
            _actions.Process(actionConfig.Actions);
        }

        public bool CheckRequire(MetaActionConfig actionConfig)
        {
            return _conditions.Check(actionConfig.Require);
        }*/
    }
    
}