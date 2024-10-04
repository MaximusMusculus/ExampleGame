using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine.Assertions;

namespace Meta
{
    /// <summary>
    /// Model-> Wrapped?
    /// </summary>
    public class MetaModel : IActionProcessor
    {
        public IInventory Inventory => _inventory;
        public IUnits Units => _units;
        public IQuests Quests => _quests;
        public IConditionProcessor ConditionProcessor => _conditions;

        private readonly IInventoryController _inventory;
        private readonly IUnitsController _units;
        private readonly QuestsController _quests;
        
        //Events
        //.....
        private readonly IConditionProcessor _conditions;
        public MetaConfig Config { get; }


        private readonly IActionProcessor _executeActionProcessor;
        private readonly IActionProcessor _questActionProcessor;
        
        public MetaModel(MetaConfig config, MetaDto data, IMetaControllersFactory controllersFactory)
        {
            Config = config;
            _inventory = controllersFactory.CreateInventoryController(data.Items);
            _units = controllersFactory.CreateUnitsController(data.Units);

            _conditions = controllersFactory.CreateConditionProcessor(_inventory, _units);
            _executeActionProcessor = controllersFactory.CreateActionProcessor(_inventory, _units);
            
            var fact = new QuestProcessorsProcessorsFactory(_conditions);
            _quests = controllersFactory.CreateQuestsController(config.Quests, data.Quests, fact, _executeActionProcessor);
            _questActionProcessor = _quests;
            
            _quests.AddNewQuest(MapTestId.QuestAddUnitGunner.Id());
            _quests.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
        }

        public void DoAction(Id group, int index)
        {
            foreach (var actionsGroup in Config.ActionsGroups)
            {
                if (actionsGroup.TypeGroup.Equals(group))
                {
                    var action = actionsGroup.Actions[index];
                    Assert.IsTrue(_conditions.Check(action.Require));
                    Process(action.Actions);
                }
            }
        }

        public void Process(IActionConfig actionConfig)
        {
            _executeActionProcessor.Process(actionConfig);
            _questActionProcessor.Process(actionConfig);
        }
    }
    
}