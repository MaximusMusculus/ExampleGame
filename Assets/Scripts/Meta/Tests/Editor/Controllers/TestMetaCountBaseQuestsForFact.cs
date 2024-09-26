using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
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
        
        [SetUp]
        public void SetUp()
        {
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
                TriggerAction = TypeMetaAction.InventoryItemSpend,
                TargetEntityId = MapTestId.Recruts.Id(),
                Reward = new UnitActionConfig {MetaAction = TypeMetaAction.UnitAdd, TypeUnit = MapTestId.UnitAssault.Id(), Count = 1}
                //Reward = new ItemActionConfig {MetaAction = TypeMetaAction.UnitAdd, TypeItem = MapTestId.UnitAssault.Id(), Count = 1} - неудобно
            });



            var actionProcessor = new ActionProcessor(_inventory, _units);
            var fact = new QuestControllerControllerFactory(new ConditionProcessor(_inventory, _units));
            _questsController = new QuestsController(_questsData, questsConfig, fact, actionProcessor);
            _actionProcessor = new ActionProcessorFacade(actionProcessor, _questsController);
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
            var spend50 = new ItemActionConfig {MetaAction = TypeMetaAction.InventoryItemSpend, TypeItem = MapTestId.Recruts.Id(), Count = 50};
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
            var spend100 = new ItemActionConfig {MetaAction = TypeMetaAction.InventoryItemSpend, TypeItem = MapTestId.Recruts.Id(), Count = 100};
            _questsController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
            _actionProcessor.Process(spend100);
            Assert.IsTrue(_questsData.CountBasedQuest.First().IsCompleted);
            
            _questsController.ClaimReward(_questsData.CountBasedQuest.First());
            Assert.AreEqual(1, _unitsData.Count);
            Assert.IsTrue(_questsData.CountBasedQuest.First().IsRewarded);
        }


    }
}