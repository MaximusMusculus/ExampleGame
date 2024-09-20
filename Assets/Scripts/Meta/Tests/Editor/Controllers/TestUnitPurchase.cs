using System;
using System.Linq;
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
    public class TestUnitPurchase
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unitBuilder = new UnitConfigBuilder();
        private readonly ActionConfigBuilder _actionConfigBuilder = new ActionConfigBuilder();
        private readonly UnitConfigBuilder _units = new UnitConfigBuilder();
        
        private MetaConfig _metaConfig;
        private MetaDto _metaDto;

        private IInventoryController _inventoryController;
        private IUnitsController _unitsController;
        private IActionProcessor _actionProcessor;

        [SetUp]
        public void SetUp()
        {
            _metaDto = new MetaDto();
            _metaConfig = _metaBuilder.NewConfig()
                .AddItemConfig(MapTestId.Scrup.Id(), 100, 100)
                .AddItemConfig(MapTestId.Recruts.Id(), 100, 100)
                .AddUnitConfig(_unitBuilder.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build()).Build();

            _inventoryController = new InventoryController(_metaConfig.InventoryItems, _metaDto.Items);
            _unitsController = new UnitsController(_metaConfig.Units, _metaDto.Units);
            
            //_actionProcessor = new ActionProcessor(_inventoryController, _unitsController, actionGroupCountController? - т.е. сущность, кот-я будет знать экшен. и хендлить его.
            //что то вроде selfCountCost. Проще всего на эвенте его рассмотреть.
            
            
            
            _actionProcessor = new ActionProcessor(_inventoryController, _unitsController);
        }

        [Test]
        public void TestUnitPurchase_InventorySpendAndUnitAdd()
        {
            var purchaseUnitAction = _actionConfigBuilder.NewAction()
                    .InventoryItemSpend(MapTestId.Recruts.Id(), 10)
                    .InventoryItemSpend(MapTestId.Scrup.Id(), 20)
                    .UnitAdd(_units.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build(), 1).Build();
            
            _actionProcessor.Process(purchaseUnitAction);
            
            Assert.AreEqual(90, _inventoryController.GetCount(MapTestId.Recruts.Id()));
            Assert.AreEqual(80, _inventoryController.GetCount(MapTestId.Scrup.Id()));
            Assert.AreEqual(1, _metaDto.Units.FirstOrDefault(s=>s.UnitType ==MapTestId.Unit_1.Id())!.Count);
        }
        
        [Test]
        public void TestUnitPurchase_InventorySpendAndUnitAdd_Fail()
        {
            var purchaseUnitAction = _actionConfigBuilder.NewAction()
                .InventoryItemSpend(MapTestId.Recruts.Id(), 10)
                .InventoryItemSpend(MapTestId.Scrup.Id(), 200)
                .UnitAdd(_units.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build(), 1).Build();

            Assert.Throws<InvalidOperationException>(() => _actionProcessor.Process(purchaseUnitAction));
        }


    }
}