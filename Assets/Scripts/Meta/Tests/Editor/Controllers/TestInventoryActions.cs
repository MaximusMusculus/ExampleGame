using System;
using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using NUnit.Framework;


namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    //?? такие же группы на кондишены
    public class TestInventoryActions
    {
        private IInventoryController _inventoryController;
        private Id _itemId1;
        private Id _itemId2;
        private ItemConfig _item2;

        private InventoryActionsProcessor _actionsProcessor;
        private ActionConfigFactory _actionsFactory;
        
        [SetUp]
        public void SetUp()
        {
            _actionsFactory = new ActionConfigFactory();
            var item1 = new ItemConfig {Item = 1, DefaultCount = 10, MaxCount = 100};
            var item2 = new ItemConfig {Item = 2, DefaultCount = 5, MaxCount = 50};
            var configs = new List<ItemConfig> {item1, item2};

            var items = new List<ItemDto>();
  
            _inventoryController = new InventoryController(configs, items);
            _actionsProcessor = new InventoryActionsProcessor(_inventoryController);

            _itemId1 = item1.Item;
            _itemId2 = item2.Item;
            _item2 = item2;
        }
        

        [Test]
        public void Add_IncreasesCountWithinLimit()
        {
            var action = _actionsFactory.CreateItemAddAction(_itemId1, 20);
            _actionsProcessor.Process(action);
            Assert.AreEqual(30, _inventoryController.GetCount(_itemId1));
        }
        
        [Test]
        public void Add_ExceedsLimit_SetsToLimit()
        {
            _actionsProcessor.Process(_actionsFactory.CreateItemAddAction(_itemId1, 200));
            Assert.AreEqual(100, _inventoryController.GetCount(_itemId1));
        }
        
        [Test]
        public void Spend_DecreasesCount()
        {
            _actionsProcessor.Process(_actionsFactory.CreateItemSpendAction(_itemId1, 5));
            Assert.AreEqual(5, _inventoryController.GetCount(_itemId1));
        }
        
        [Test]
        public void Spend_NotEnoughItems_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _actionsProcessor.Process(_actionsFactory.CreateItemSpendAction(_itemId1, 15)));
        }
        
        [Test]
        public void ExpandLimit_IncreasesLimit()
        {
            _actionsProcessor.Process(_actionsFactory.CreateItemExpandLimitAction(_itemId1, 200));
            Assert.AreEqual(300, _inventoryController.GetLimit(_itemId1));
        }

    }
}