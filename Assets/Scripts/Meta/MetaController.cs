using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta
{
    /// <summary>
    /// Model-> Wrapped?
    /// </summary>
    public class MetaModel
    {
        public IInventory Inventory => _inventory;
        public IUnits Units => _units;
        
        public IConditionProcessor ConditionProcessor => _conditions;
        public IActionProcessor ActionProcessor => _actions;

        
        private readonly IInventoryController _inventory;
        private readonly IUnitsController _units;
        
        //Events
        //.....
        private readonly IActionProcessor _actions;
        private readonly IConditionProcessor _conditions;
        public MetaConfig Config { get; }
        
        public MetaModel(MetaConfig config, MetaDto data, IMetaControllersFactory controllersFactory)
        {
            Config = config;
            _inventory = controllersFactory.CreateInventoryController(data.Items);
            _units = controllersFactory.CreateUnitsController(data.Units);

            _actions = controllersFactory.CreateActionProcessor(_inventory, _units);
            _conditions = controllersFactory.CreateConditionProcessor(_inventory, _units);
        }

        public void DoAction(Id group, int index)
        {
            foreach (var actionsGroup in Config.ActionsGroups)
            {
                if (actionsGroup.TypeGroup.Equals(group))
                {
                    var action = actionsGroup.Actions[index];
                    Assert.IsTrue(_conditions.Check(action.Require));
                    _actions.Process(action.Actions);
                }
            }
        }
    }
    
}