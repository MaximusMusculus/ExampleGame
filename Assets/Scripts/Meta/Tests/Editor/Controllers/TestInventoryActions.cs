using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Imp;
using Meta.Models;
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
        
        [SetUp]
        public void SetUp()
        {
            var item1 = new ItemConfig {Item = 1, DefaultCount = 10, MaxCount = 100};
            var item2 = new ItemConfig {Item = 2, DefaultCount = 5, MaxCount = 50};
            var configs = new List<ItemConfig> {item1, item2};

            var items = new List<ItemDto>();
  
            _inventoryController = new InventoryController(configs, items);

            _itemId1 = item1.Item;
            _itemId2 = item2.Item;
            _item2 = item2;
        }

        [Test]
        public void Add_IncreasesCountWithinLimit()
        {
            var action = new InventoryItemAddAction(_inventoryController);
            action.Process(new ItemActionConfig {TypeItem = _itemId1, Count = 20});
            Assert.AreEqual(30, _inventoryController.GetCount(_itemId1));
        }
        
        [Test]
        public void Add_ExceedsLimit_SetsToLimit()
        {
            var action = new InventoryItemAddAction(_inventoryController);
            action.Process(new ItemActionConfig {TypeItem = _itemId1, Count = 200});
            Assert.AreEqual(100, _inventoryController.GetCount(_itemId1));
        }
        
        [Test]
        public void Spend_DecreasesCount()
        {
            var action = new InventoryItemSpendAction(_inventoryController);
            action.Process(new ItemActionConfig {TypeItem = _itemId1, Count = 5});
            Assert.AreEqual(5, _inventoryController.GetCount(_itemId1));
        }
        
        [Test]
        public void Spend_NotEnoughItems_ThrowsException()
        {
            var action = new InventoryItemSpendAction(_inventoryController);
            //Assert.Throws<NotEnoughItemsException>(() => action.Process(new ItemActionConfig {TypeItem = _itemId1, Count = 15})); ??
        }
        
        [Test]
        public void ExpandLimit_IncreasesLimit()
        {
            var action = new InventoryItemExpandLimitAction(_inventoryController);
            action.Process(new ItemActionConfig { TypeItem = _itemId1, Count = 200});
            Assert.AreEqual(300, _inventoryController.GetLimit(_itemId1));
        }

    }
}