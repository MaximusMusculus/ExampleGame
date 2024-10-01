using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Conditions;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestMetaCountBaseQuestsForFact
    {
        private IIdProvider _idProvider;
        private IActionProcessor _actionProcessor;
        
        private List<ItemDto> _itemsData;
        private List<UnitDto> _unitsData;
        
        private InventoryController _inventory;
        private UnitsController _units;
        private QuestCollectionDto _questsData;
        private QuestsController _questsController;

        private ActionConfigFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new ActionConfigFactory();
            _questsData = new QuestCollectionDto(); 
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
                TriggerAction = TypeQuest.InventoryItemSpend,
                TargetEntityId = MapTestId.Recruts.Id(),
                Reward = _factory.CreateUnitAddAction(MapTestId.UnitAssault.Id(), 1)
            });


            var actionProcessor = new ActionProcessor(_inventory, _units);
            var fact = new QuestControllerControllerFactory(new ConditionProcessor(_inventory, _units));
            _questsController = new QuestsController(questsConfig,_questsData, fact, actionProcessor);
            var autoComplete = new QuestAutoCompleteProcessorForFact(_questsData, _questsController);
            _actionProcessor = new ActionProcessorFacade(actionProcessor, _questsController, autoComplete);
        }
        
        
        [Test]
        public void TestQuestIsCreated()
        {
            _questsController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            Assert.AreEqual(1, _questsData.CountBasedQuest.Count);
        }
        
        [Test]
        public void TestQuestSpendRecrutsProgress()
        {
            var spend50 = _factory.CreateItemSpendAction(MapTestId.Recruts.Id(), 50);
            _questsController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            Assert.AreEqual(0, _questsData.CountBasedQuest.First().Value);

            _actionProcessor.Process(spend50);
            Assert.AreEqual(50, _questsData.CountBasedQuest.First().Value);
            
            _actionProcessor.Process(spend50);
            Assert.AreEqual(100, _questsData.CountBasedQuest.First().Value);
            Assert.IsTrue(_questsData.CountBasedQuest.First().IsCompleted);
        }
        
        [Test]
        public void TestQuestClaimReward()
        {
            var spend100 = _factory.CreateItemSpendAction(MapTestId.Recruts.Id(), 100);
            _questsController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            _actionProcessor.Process(spend100);
            Assert.IsTrue(_questsData.CountBasedQuest.First().IsCompleted);
            
            _questsController.ClaimReward(_questsData.CountBasedQuest.First());
            Assert.AreEqual(1, _unitsData.Count);
            Assert.IsTrue(_questsData.CountBasedQuest.First().IsRewarded);
        }

    }
    
    public class QuestAutoCompleteProcessorForFact : IActionProcessor
    {
        private readonly QuestCollectionDto _questsData;
        private readonly IQuestsController _questController;

        public QuestAutoCompleteProcessorForFact(QuestCollectionDto questsData, IQuestsController questController)
        {
            _questsData = questsData;
            _questController = questController;
        }

        public void Process(IActionConfig actionConfig)
        {
            foreach (var quest in _questsData.CountBasedQuest)
            {
                if (quest.IsCompleted && quest.IsRewarded == false)
                {
                    _questController.ClaimReward(quest);
                }
            }

            foreach (var quest in _questsData.ConditionalQuest)
            {
                if (quest.IsCompleted && quest.IsRewarded == false)
                {
                    _questController.ClaimReward(quest);
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