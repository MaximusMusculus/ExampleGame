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
    public class TestMetaActions
    {
        private readonly IMetaConfigProvider _metaConfigProvider = new TestMetaConfigProvider();
        private MetaConfig _metaConfig;
        private MetaDto _metaDto;

        //пока кусками, но уже пора объединять в одно
        private IInventoryController _inventoryController;
        private IUnitsController _unitsController;
        private IActionProcessor _actionProcessor;

        [SetUp]
        public void Setup()
        {
            _metaDto = new MetaDto();
            _metaConfig = _metaConfigProvider.GetConfig();

            _inventoryController = new InventoryController(_metaConfig.InventoryItems, _metaDto.Items);
            _unitsController = new UnitsController(_metaConfig.Units, _metaDto.Units);
            _actionProcessor = new ActionProcessor(_inventoryController, _unitsController);
        }

        [Test]
        public void TestMetaActions_TrainUnit()
        {
            _inventoryController.Add(MapTestId.Scrup.Id(), 100);
            _inventoryController.Add(MapTestId.Recruts.Id(), 100);

            var action = _metaConfig.Actions[0];
            _actionProcessor.Process(action.Spend);
            _actionProcessor.Process(action.Add);

            Assert.AreEqual(50, _inventoryController.GetCount(MapTestId.Scrup.Id()));
            Assert.AreEqual(80, _inventoryController.GetCount(MapTestId.Recruts.Id()));
            Assert.AreEqual(1, _metaDto.Units.Find(s => s.UnitType == MapTestId.Unit_1.Id())!.Count);
        }
    }
}