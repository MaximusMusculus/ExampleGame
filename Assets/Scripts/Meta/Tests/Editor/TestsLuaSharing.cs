using System;
using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Imp;
using Meta.Models;
using MoonSharp.Interpreter;
using NUnit.Framework;


namespace Meta.Tests.Editor
{
    [TestFixture]
    public class TestsLuaSharing
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
        public void TestLuaInventoryControllerWithProxy()
        {
            string scriptCode = @"inventory.Add(1, 20)";

            UserData.RegisterProxyType<InventoryProxy, IInventoryController>(r => new InventoryProxy(r));
            Script script = new Script();
            script.Globals["inventory"] = _inventoryController;
            script.DoString(scriptCode);
            Assert.AreEqual(30, _inventoryController.GetCount(_itemId1));


            //Table dump = UserData.GetDescriptionOfRegisteredTypes(true);
            //Debug.Log(JsonConvert.SerializeObject(dump));
        }

        public class InventoryProxy
        {
            public IInventoryController Inventory;

            [MoonSharpHidden]
            public InventoryProxy(IInventoryController inventory)
            {
                Inventory = inventory;
            }

            public void Add(int id, int count)
            {
                Inventory.Add((ushort) id, count);
            }
        }


        [Test]
        public void TestLuaInventoryByGlobal()
        {
            string scriptCode = @"AddInventoryItem(1, 20)"; //инструкция из данных.
            Script script = new Script();
            script.Globals["AddInventoryItem"] = (Action<int, int>) AddInventoryItem;

            script.DoString(scriptCode);
            Assert.AreEqual(30, _inventoryController.GetCount(_itemId1));
        }

        public void AddInventoryItem(int id, int count)
        {
            _inventoryController.Add((ushort) id, count);
        }
    }
}