using System.Linq;
using Meta.Configs;
using Meta.Configs.TestConfiguration;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Conditions;
using Meta.Controllers.Imp;
using Meta.Models;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestConditions
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly ActionConfigBuilder _actionConfigBuilder = new ActionConfigBuilder(); //condition

        private MetaConfig _metaConfig;
        private MetaDto _metaDto;

        private IInventoryController _inventoryController;
        private IConditionProcessor _conditionProcessor;

        [SetUp]
        public void SetUp()
        {
            _metaDto = new MetaDto();
            _metaBuilder.NewConfig()
                .AddItemConfig(MapTestId.Scrup.Id(), 100, 100)
                .AddItemConfig(MapTestId.Recruts.Id(), 100, 100);
            
            _metaConfig = _metaBuilder.Build();
            _inventoryController = new InventoryController(_metaConfig.InventoryItems, _metaDto.Items);
            _conditionProcessor = new ConditionProcessor(_inventoryController);
        }

        [Test]
        public void TestUnitPurchase_InventorySpendAndUnitAdd()
        {
            /*var purchaseUnitAction = _actionConfigBuilder.NewAction()
                    .InventoryItemSpend(MapTestId.Recruts.Id(), 10)
                    .InventoryItemSpend(MapTestId.Scrup.Id(), 20)
                    .UnitAdd(_units.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build(), 1).Build();
            
            _actionProcessor.Process(purchaseUnitAction);
            
            Assert.AreEqual(90, _inventoryController.GetCount(MapTestId.Recruts.Id()));
            Assert.AreEqual(80, _inventoryController.GetCount(MapTestId.Scrup.Id()));
            Assert.AreEqual(1, _metaDto.Units.FirstOrDefault(s=>s.UnitType ==MapTestId.Unit_1.Id())!.Count);*/
        }


    }
}