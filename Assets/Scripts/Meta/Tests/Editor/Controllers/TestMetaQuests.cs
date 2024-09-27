using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestMetaCountBaseQuests
    {
        private IIdProvider _idProvider;
        private IActionProcessor _actionProcessor;
        private QuestController _questController;

        private List<ItemDto> _itemsData;
        private List<UnitDto> _unitsData;
        private QuestsDto _questsData;

        private InventoryController _inventory;
        private UnitsController _units;

        [SetUp]
        public void SetUp()
        {
            _questsData = new QuestsDto();
            _itemsData = new List<ItemDto>();
            _unitsData = new List<UnitDto>();
            _idProvider = new IdProvider(new IdProviderDto());

            var itemsConfig = new List<ItemConfig> {new ItemConfig {Item = MapTestId.Recruts.Id(), DefaultCount = 200}};
            _inventory = new InventoryController(itemsConfig, _itemsData);
            
            var unitsConfig = new List<UnitConfig> {new UnitConfig {UnitType = MapTestId.UnitAssault.Id()}};
            _units = new UnitsController(unitsConfig, _unitsData);

            var questsConfig = new QuestCollectionConfig();
            questsConfig.Add(new QuestCountBasedConfig
            {
                QuestId = MapTestId.QuestSpendRecruts.Id(),
                TargetValue = 100,
                TriggerAction = TypeMetaAction.InventoryItemSpend,
                TargetEntityId = MapTestId.Recruts.Id(),
                Reward = new UnitActionConfig {MetaAction = TypeMetaAction.UnitAdd, TypeUnit = MapTestId.UnitAssault.Id(), Count = 1}
            });
            
            
            var execute = new ActionProcessor(_inventory, _units);
            _questController = new QuestController(questsConfig, _questsData, _idProvider, execute);
            var autoComplete = new QuestAutoCompleteProcessor(_questsData, _questController);
            _actionProcessor = new ActionProcessorFacade(execute, new QuestMetaCountBasedProcessor(questsConfig.CountBased, _questsData), autoComplete);
           
            

        }

        [Test]
        public void TestQuestIsCreated()
        {
            _questController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            Assert.AreEqual(1, _questsData.Quests.Count);
        }

        [Test]
        public void TestQuestSpendRecrutsProgress()
        {
            var spend50 = new ItemActionConfig {MetaAction = TypeMetaAction.InventoryItemSpend, TypeItem = MapTestId.Recruts.Id(), Count = 50};
            _questController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            
            _questController.TryGetCount(MapTestId.QuestSpendRecruts.Id(), out var counter);
            _actionProcessor.Process(spend50);
            Assert.AreEqual(50, _questsData.Counters.First().Value); 
                
            _actionProcessor.Process(spend50);
            var quest = _questController.GetAll().First();
            Assert.AreEqual(100, _questsData.Counters.First().Value);
            Assert.IsTrue(quest.IsCompleted);
        }
        
        [Test]
        public void TestQuestClaimReward()
        {
            var spend100 = new ItemActionConfig {MetaAction = TypeMetaAction.InventoryItemSpend, TypeItem = MapTestId.Recruts.Id(), Count = 100};
            _questController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            _actionProcessor.Process(spend100);
            Assert.IsTrue(_questsData.Quests.First().IsCompleted);
            //_questController.ClaimReward(_questsData.Quests.First().Id);
            
            
            Assert.AreEqual(1, _unitsData.First().Count);
            Assert.IsTrue(_questsData.Quests.First().IsRewarded);
        }
    }

    public class QuestAutoCompleteProcessor : IActionProcessor
    {
        private readonly QuestsDto _questsData;
        private readonly IQuestsController _questController;

        public QuestAutoCompleteProcessor(QuestsDto questsData, IQuestsController questController)
        {
            _questsData = questsData;
            _questController = questController;
        }

        public void Process(IActionConfig actionConfig)
        {
            foreach (var quest in _questsData.Quests)
            {
                if (quest.IsCompleted && quest.IsRewarded == false)
                {
                    _questController.ClaimReward(quest.Id);
                }
            }
        }
    }
    
    public class ActionProcessorFacade : IActionProcessor
    {
        private readonly IActionProcessor _executeActionProcessor;
        private readonly IActionProcessor _questActionProcessor;
        private readonly IActionProcessor _questAutoCompleteProcessor;

        public ActionProcessorFacade(IActionProcessor executeActionProcessor, IActionProcessor questActionProcessor, IActionProcessor autoComplete)
        {
            _executeActionProcessor = executeActionProcessor;
            _questActionProcessor = questActionProcessor;
            _questAutoCompleteProcessor = autoComplete;
        }

        public void Process(IActionConfig actionConfig)
        {
            _executeActionProcessor.Process(actionConfig);
            _questActionProcessor.Process(actionConfig);
            _questAutoCompleteProcessor.Process(actionConfig);
        }
    }
}