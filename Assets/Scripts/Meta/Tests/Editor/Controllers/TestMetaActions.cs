using System;
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
    public class TestMetaActions
    {
        private readonly IMetaConfigProvider _metaConfigProvider = new TestMetaConfigProvider();
        private MetaConfig _metaConfig;
        private MetaDto _metaDto;

        //пока кусками, но уже пора объединять в одно
        private IInventoryController _inventoryController;
        private IUnitsController _unitsController;
        private IActionProcessor _actionProcessor;
        private IConditionProcessor _conditionProcessor;
        
        

        [SetUp]
        public void Setup()
        {
            _metaDto = new MetaDto();
            _metaConfig = _metaConfigProvider.GetConfig();

            _inventoryController = new InventoryController(_metaConfig.InventoryItems, _metaDto.Items);
            _unitsController = new UnitsController(_metaConfig.Units, _metaDto.Units);
            _actionProcessor = new ActionProcessor(_inventoryController, _unitsController);
            _conditionProcessor = new ConditionProcessor(_inventoryController, _unitsController);
        }

        [Test]
        public void TestMetaActions_TrainUnit()
        {
            _inventoryController.Add(MapTestId.Scrup.Id(), 100);
            _inventoryController.Add(MapTestId.Recruts.Id(), 100);

            var action = _metaConfig.Actions[0];
            _actionProcessor.Process(action.Actions);

            Assert.AreEqual(50, _inventoryController.GetCount(MapTestId.Scrup.Id()));
            Assert.AreEqual(80, _inventoryController.GetCount(MapTestId.Recruts.Id()));
            Assert.AreEqual(1, _metaDto.Units.Find(s => s.UnitType == MapTestId.UnitGunner.Id())!.Count);
        }
        
        [Test]
        public void TestMetaActions_TrainUnitToUnitsLimit()
        {
            var startCount = 5000;
            _inventoryController.ExpandLimit(MapTestId.Scrup.Id(), startCount);
            _inventoryController.ExpandLimit(MapTestId.Recruts.Id(), startCount);
            _inventoryController.Add(MapTestId.Scrup.Id(), startCount);
            _inventoryController.Add(MapTestId.Recruts.Id(), startCount);

            var action = _metaConfig.Actions[0];
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(_conditionProcessor.Check(action.Require));
                _actionProcessor.Process(action.Actions);
            }
            Assert.IsFalse(_conditionProcessor.Check(action.Require));
        }
        
        
        [Test]
        public void TestMetaActions_CheckAndTrainUnit()
        {
            _inventoryController.Add(MapTestId.Scrup.Id(), 10);
            _inventoryController.Add(MapTestId.Recruts.Id(), 100);

            var action = _metaConfig.Actions[0];
            //как проверить наличие? Перед действием?
            //По идее это будет делать вьюха.
            //А я просто разверну 2ю модель и буду на ней смотреть. Если она при выполнения действия не падает - то все ок ^_^
            //это избавит от кучи проверок

            Assert.IsFalse(_conditionProcessor.Check(action.Require));
            Assert.Throws<InvalidOperationException>(() => _actionProcessor.Process(action.Actions));
        }
    }
}