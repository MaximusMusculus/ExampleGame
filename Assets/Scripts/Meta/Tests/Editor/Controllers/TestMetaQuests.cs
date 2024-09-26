using System;
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
    public class TestMetaQuests
    {
        private MetaConfig _config;
        private MetaDto _data;
        private IIdProvider _idProvider;

        private IActionProcessor _actionProcessor;
        private QuestController _questController;
        private ActionCollectionConfigBuilder _actionBuilder = new ActionCollectionConfigBuilder();

        [SetUp]
        public void SetUp()
        {
            _idProvider = new IdProvider(new IdProviderDto {nextId = 100});
            _data = new MetaDto();
            _config = new TestMetaConfigProvider().GetConfig();

            var inventory = new InventoryController(_config.InventoryItems, _data.Items);
            inventory.Add(MapTestId.Recruts.Id(), 1000);
            
            var units = new UnitsController(_config.Units, _data.Units);
            var metaConditions = new ConditionProcessor(inventory, units);

            var actionProcessor = new ActionProcessorChain();
            _actionProcessor = actionProcessor;
            actionProcessor.AddProcessor(new ActionProcessor(inventory, units));
            actionProcessor.AddProcessor(new QuestMetaConditionalProcessor(_config.Quests.ConditionBased, _data.PlayerQuests, metaConditions));
            actionProcessor.AddProcessor(new QuestMetaCountBasedProcessor(_config.Quests.CountBased, _data.PlayerQuests));

            _questController = new QuestController(_config.Quests, _data.PlayerQuests, _idProvider, actionProcessor);
            _questController.AddNewQuest(MapTestId.QuestSpendRecruts.Id());
        }

        [Test]
        public void TestQuestIsCreated()
        {
            Assert.AreEqual(3, _data.PlayerQuests.Quests.Count);
        }

        [Test]
        public void TestQuestSpendRecrutsProgress()
        {
            _questController.TryGetCount(MapTestId.QuestSpendRecruts.Id(), out var counter);
            _actionProcessor.Process(_actionBuilder.NewAction().InventoryItemSpend(MapTestId.Recruts.Id(), 100).Build());

            _questController.TryGetCount(MapTestId.QuestSpendRecruts.Id(), out counter);
            Assert.AreEqual(100, _data.PlayerQuests.Counters.First().Value);

            _actionProcessor.Process(_actionBuilder.NewAction().InventoryItemSpend(MapTestId.Recruts.Id(), 100).Build());

            var quest = _questController.GetAll().First();
            Assert.IsTrue(quest.IsCompleted);
        }




        private class ActionProcessorChain : IActionProcessor
        {
            private List<IActionProcessor> _processors = new List<IActionProcessor>();
            
            public void AddProcessor(IActionProcessor processor)
            {
                _processors.Add(processor);
            }

            public void Process(IActionConfig actionConfig)
            {
                foreach (var processor in _processors)
                {
                    processor.Process(actionConfig);
                }
            }
        }
        
    }
}