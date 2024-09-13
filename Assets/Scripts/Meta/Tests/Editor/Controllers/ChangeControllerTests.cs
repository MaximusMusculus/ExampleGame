using System;
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
    public class ChangeControllerTests
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
        
        [Test]
        public void GivenItemWithCount_WhenCountIsSpent_ThenCorrectCountRemains()
        {
            _itemsController.Add(_itemRecruits, 100);
            _changeController.Process(new ConfigChangeSpendItem {TargetItem = _itemRecruits, Count = 50});
            Assert.AreEqual(50, _itemsController.GetCount(_itemRecruits));
        }

        [Test]
        public void GivenItemWithCount_WhenCountIsSpentArray_ThenCorrectCountRemains()
        {
            _itemsController.Add(_itemRecruits, 100);
            _changeController.Process(new ChangesConfig
            {
                Changes = new List<ChangeConfig>
                {
                    new ConfigChangeSpendItem {TargetItem = _itemRecruits, Count = 50},
                    new ConfigChangeSpendItem {TargetItem = _itemRecruits, Count = 30},
                }
            });
            Assert.AreEqual(20, _itemsController.GetCount(_itemRecruits));
        }
        [Test]
        public void GivenItemWithCount_WhenPurchaseUnits_ThenCorrectCountRemainsAndUnits()
        {
            _itemsController.Add(_itemRecruits, 100);
            _itemsController.Add(_itemIron, 150);
            
            
            var addUnit = new ConfigChangeAddUnit
            {
                UnitType = _unitType,
                Progression = new UnitProgressionDto {HealthLevel = 1},
                Count = 5
            };
            var costItems = new ChangesConfig
            {
                Changes = new List<ChangeConfig>
                {
                    new ConfigChangeSpendItem {TargetItem = _itemRecruits, Count = 50},
                    new ConfigChangeSpendItem {TargetItem = _itemIron, Count = 50},
                }
            };
            
            _changeController.Process(new ChangesConfig {Changes = new List<ChangeConfig> {addUnit, costItems}});
            Assert.AreEqual(5, _unitsController.GetUnits().First().Count);
            Assert.AreEqual(50, _itemsController.GetCount(_itemRecruits));
            Assert.AreEqual(100, _itemsController.GetCount(_itemIron));
        }


        //Тест успешной покупки юнита -наемника
        //Тест покупки юнита, когда не хватает ресурсов -
        //тест успешного создания юнита игроком
        //таст создания юнита игроком, когда не хватает ресурсов
        //тест создания юнита игроком, когда достигнут лимит (какой и где?) - статика в конфиге?



        //--- config
        public enum TypeChange
        {
            AddItem,
            SpendItem,
            AddUnit,
            ChangesArray,
        }

        public abstract class ChangeConfig
        {
            public abstract TypeChange TypeChange { get; }
        }
        
        public class ChangesConfig : ChangeConfig
        {
            public List<ChangeConfig> Changes = new List<ChangeConfig>();
            public override TypeChange TypeChange => TypeChange.ChangesArray;
        }

        public class ConfigChangeAddItem : ChangeConfig
        {
            public override TypeChange TypeChange => TypeChange.AddItem;
            public Id TargetItem;
            public int Count;
        }

        public class ConfigChangeSpendItem : ChangeConfig
        {
            public override TypeChange TypeChange => TypeChange.SpendItem;
            public Id TargetItem;
            public int Count;
        }

        public class ConfigChangeAddUnit : ChangeConfig
        {
            public override TypeChange TypeChange => TypeChange.AddUnit;
            public Id UnitType;
            public UnitProgressionDto Progression;
            public int Count;
        }

        //-- runtime
        public interface IChangeController
        {
            void Process(ChangeConfig change);
        }

        public interface IChangeControllersFactory
        {
            IChangeController Create(ChangeConfig change);
        }
        public class ChangeControllersFactory : IChangeControllersFactory
        {
            private readonly IInventoryController _inventoryController;
            private readonly IUnitsController _unitsController;

            public ChangeControllersFactory(IInventoryController inventoryController, IUnitsController unitsController)
            {
                _inventoryController = inventoryController;
                _unitsController = unitsController;
            }

            public IChangeController Create(ChangeConfig change)
            {
                return change.TypeChange switch
                {
                    TypeChange.AddItem => new ChangeAddItemController(_inventoryController),
                    TypeChange.SpendItem => new ChangeSpendItemController(_inventoryController),
                    TypeChange.AddUnit => new ChangeAddUnitController(_unitsController),
                    _ =>   throw new ArgumentException("Processor not found for change type: " + change.TypeChange)
                };
            }
        }
        
        public class ChangeArrayController : IChangeController
        {
            private readonly IChangeController _changeController;

            public ChangeArrayController(IChangeController changeController)
            {
                _changeController = changeController;
            }

            public void Process(ChangeConfig change)
            {
                var config = (ChangesConfig) change;
                foreach (var changeConfig in config.Changes)
                {
                    _changeController.Process(changeConfig);
                }
            }
        }
        
        public class ChangeController : IChangeController
        {
            private readonly IChangeControllersFactory _changeControllersFactory;
            private readonly Dictionary<TypeChange, IChangeController> _controllersHash;

            public ChangeController(IChangeControllersFactory changeControllersFactory)
            {
                _changeControllersFactory = changeControllersFactory;
                _controllersHash = new Dictionary<TypeChange, IChangeController>(16);
                
                //Создать на фабрике не могу, так как будет цикличная зависимость, пока думаю что с этим делать и нужно ли это.
                _controllersHash.Add(TypeChange.ChangesArray, new ChangeArrayController(this));
            }

            public void Process(ChangeConfig change)
            {
                if (_controllersHash.TryGetValue(change.TypeChange, out var processor) == false)
                {
                    processor = _changeControllersFactory.Create(change);
                    _controllersHash.Add(change.TypeChange, processor);
                }
                processor.Process(change);
            }
        }

        public class ChangeAddItemController : IChangeController
        {
            private readonly IInventoryController _inventoryController;

            public ChangeAddItemController(IInventoryController inventoryController)
            {
                _inventoryController = inventoryController;
            }

            public void Process(ChangeConfig change)
            {
                var configChangeAddItem = (ConfigChangeAddItem) change;
                _inventoryController.Add(configChangeAddItem.TargetItem, configChangeAddItem.Count);
            }
        }

        public class ChangeSpendItemController : IChangeController
        {
            private readonly IInventoryController _inventoryController;

            public ChangeSpendItemController(IInventoryController inventoryController)
            {
                _inventoryController = inventoryController;
            }

            public void Process(ChangeConfig change)
            {
                var configChangeSpendItem = (ConfigChangeSpendItem) change;
                _inventoryController.Spend(configChangeSpendItem.TargetItem, configChangeSpendItem.Count);
            }
        }

        public class ChangeAddUnitController : IChangeController
        {
            private readonly IUnitsController _unitsController;

            public ChangeAddUnitController(IUnitsController unitsController)
            {
                _unitsController = unitsController;
            }

            public void Process(ChangeConfig change)
            {
                var configChangeAddUnit = (ConfigChangeAddUnit) change;
                _unitsController.Add(configChangeAddUnit.UnitType, configChangeAddUnit.Progression, configChangeAddUnit.Count);
            }
        }
    }
}