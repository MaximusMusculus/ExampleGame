using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Models;
using NUnit.Framework;


namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestsChangeController
    {
        private IInventoryController _itemsController;
        private IUnitsController _unitsController;
        private IChangeController _changeController;

        private readonly Id _itemRecruits = 1;
        private readonly Id _itemIron = 2;
        private readonly Id _unitType = 3;

        private List<ItemDto> _itemsDto;
        private List<UnitDto> _unitsDto;

        [SetUp]
        public void SetUp()
        {
            _itemsDto = new List<ItemDto>();
            _unitsDto = new List<UnitDto>();

            var itemsConfigs = new List<ItemConfig>
            {
                new ItemConfig {Item = _itemRecruits, MaxCount = int.MaxValue},
                new ItemConfig {Item = _itemIron, MaxCount = int.MaxValue}
            };

            var unitConfigs = new List<UnitConfig>
            {
                new UnitConfig {UnitType = _unitType, IsCanUpgrade = true},
            };

            _itemsController = new InventoryController(itemsConfigs, _itemsDto);
            _unitsController = new UnitsController(unitConfigs, _unitsDto);
            _changeController = new ChangeController(new ChangeControllersFactory(_itemsController, _unitsController));
        }
        
        //Тест успешной покупки юнита -наемника
        //Тест покупки юнита, когда не хватает ресурсов -
        //тест успешного создания юнита игроком
        //таст создания юнита игроком, когда не хватает ресурсов
        //тест создания юнита игроком, когда достигнут лимит (какой и где?) - статика в конфиге?
        

        [Test]
        public void GivenItemWithCount_WhenCountIsSpent_ThenCorrectCountRemains()
        {
            _itemsController.Add(_itemRecruits, 100);
            _changeController.Process(new ChangeSpendItemConfig {TargetItem = _itemRecruits, Count = 50});
            Assert.AreEqual(50, _itemsController.GetCount(_itemRecruits));
        }

        [Test]
        public void GivenItemWithCount_WhenCountIsSpentArray_ThenCorrectCountRemains()
        {
            _itemsController.Add(_itemRecruits, 100);
            _changeController.Process(new ChangesArrayConfig
            {
                Changes = new List<ChangeConfig>
                {
                    new ChangeSpendItemConfig {TargetItem = _itemRecruits, Count = 50},
                    new ChangeSpendItemConfig {TargetItem = _itemRecruits, Count = 30},
                }
            });
            Assert.AreEqual(20, _itemsController.GetCount(_itemRecruits));
        }

        [Test]
        public void GivenItemWithCount_WhenPurchaseUnits_ThenCorrectCountRemainsAndUnits()
        {
            _itemsController.Add(_itemRecruits, 100);
            _itemsController.Add(_itemIron, 150);


            var addUnit = new ChangeAddUnitConfig
            {
                UnitType = _unitType,
                Progression = new UnitProgressionDto {HealthLevel = 1},
                Count = 5
            };
            var costItems = new ChangesArrayConfig
            {
                Changes = new List<ChangeConfig>
                {
                    new ChangeSpendItemConfig {TargetItem = _itemRecruits, Count = 50},
                    new ChangeSpendItemConfig {TargetItem = _itemIron, Count = 50},
                }
            };

            _changeController.Process(new ChangesArrayConfig {Changes = new List<ChangeConfig> {addUnit, costItems}});
            Assert.AreEqual(5, _unitsController.GetUnits().First().Count);
            Assert.AreEqual(50, _itemsController.GetCount(_itemRecruits));
            Assert.AreEqual(100, _itemsController.GetCount(_itemIron));
        }
    }
}