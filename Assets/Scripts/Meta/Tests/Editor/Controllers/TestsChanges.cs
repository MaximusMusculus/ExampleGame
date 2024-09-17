using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Models;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestsChanges
    {
        private readonly Id _softItem = 1;
        private List<ItemDto> _itemsDto;

        private IChangeVisitor _changeAddController;
        private IChangeVisitor _changeSpendController;
        private ChangesCheckCanSpendController _changeCheckController;
        private IChangeEntityConfig _change;
        private IInventoryController _inventory;
        
        [SetUp]
        public void SetUp()
        {
            _itemsDto = new List<ItemDto>();
            var itemsConfigs = new List<ItemConfig>
            {
                new ItemConfig {Item = _softItem, DefaultCount  = 150, MaxCount = int.MaxValue}
            };
            
            _change = new ChangeItemConfig(_softItem, 100);
            _inventory = new InventoryController(itemsConfigs, _itemsDto);
            _changeAddController = new ChangesAddController(_inventory, null);
            _changeSpendController = new ChangesSpendController(_inventory, null);
            _changeCheckController = new ChangesCheckCanSpendController(_inventory, null);
        }
        
        [Test]
        public void ChangeAddItemWithCount_ThenCorrectCountRemains()
        {
            _change.Visit(_changeAddController);
            Assert.AreEqual(250,  _inventory.GetCount(_softItem));
        }
        
        [Test]
        public void ChangeSpendItemWithCount_ThenCorrectCountRemains()
        {
            _change.Visit(_changeSpendController);
            Assert.AreEqual(50,  _inventory.GetCount(_softItem));
        }
        
        [Test]
        public void ChangeCheckCanSpendItemWithCount_ThenCorrectCountRemains()
        {
            using (_changeCheckController)
            {
                _change.Visit(_changeCheckController);
                Assert.IsTrue(_changeCheckController.Result);
            }
        }
        
        [Test]
        public void ChangeCheckCantSpendItemWithCount_ThenCorrectResult()
        {
            var notCorrectChange = new ChangeItemConfig(_softItem, 200);
            using (_changeCheckController)
            {
                notCorrectChange.Visit(_changeCheckController);
                Assert.IsFalse(_changeCheckController.Result);
            }
        }
        
        

    }
}