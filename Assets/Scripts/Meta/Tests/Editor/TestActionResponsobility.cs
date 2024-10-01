using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using NUnit.Framework;


namespace Meta.Tests.Editor
{
    [TestFixture]
    public class TestActionResponsibility
    {
        private IIdProvider _idProvider;
        private IActionProcessor _actionProcessor;
        private QuestController _questController;

        private List<ItemDto> _itemsData;
        private List<UnitDto> _unitsData;
        private QuestsDto _questsData;

        private InventoryController _inventory;
        private UnitsController _units;
        private ActionConfigFactory _actionsFactory;

        [SetUp]
        public void SetUp()
        {
            _questsData = new QuestsDto();
            _itemsData = new List<ItemDto>();
            _unitsData = new List<UnitDto>();
            _idProvider = new IdProvider(new IdProviderDto());

            var itemsConfig = new List<ItemConfig> {new ItemConfig {Item = MapTestId.Recruts.Id(), DefaultCount = 200}};
            _inventory = new InventoryController(itemsConfig, _itemsData);

            var unitsConfig = new List<UnitConfig> {new UnitConfig {UnitType = MapTestId.UnitAssault.Id()}};
            _units = new UnitsController(unitsConfig, _unitsData);

            var questsConfig = new QuestCollectionConfig();
            questsConfig.Add(new QuestCountBasedConfig
            {
                QuestId = MapTestId.QuestSpendRecruts.Id(),
                TargetValue = 100,
                TriggerAction = TypeQuest.InventoryItemSpend,
                TargetEntityId = MapTestId.Recruts.Id(),
                Reward = _actionsFactory.CreateUnitAddAction(MapTestId.UnitAssault.Id(),1)
            });
            
            var execute = new ActionProcessor(_inventory, _units);
        }
        
        public interface IQuestInventory
        {
            void TrackInventoryAmountAdded(Id item, int amount);
            void TrackInventoryAmountSpend(Id item, int amount);
            void TrackInventoryExpandLimit(Id item, int amount);
        }
        public interface IQuestUnits
        {
            void TrackUnitAdded(Id unit, int amount, int level);
            void TrackUnitSpend(Id unit, int amount, int level);
        }
        
        public class QuestService : IQuestInventory, IQuestUnits
        {
            public void TrackInventoryAmountAdded(Id item, int amount)
            {
                //questType -> 
            }
            public void TrackInventoryAmountSpend(Id item, int amount) {  }
            public void TrackInventoryExpandLimit(Id item, int amount) {  }
            
            public void TrackUnitAdded(Id unit, int amount, int level) {  }
            public void TrackUnitSpend(Id unit, int amount, int level) {  }
        }
        
        public class UnitsFacade : IUnitsController
        {
            private readonly IQuestUnits _questsService;
            private readonly IUnitsController _unitsController;

            public UnitsFacade(IQuestUnits questsService, IUnitsController unitsController)
            {
                _questsService = questsService;
                _unitsController = unitsController;
            }

            public void Add(Id unitType, int count)
            {
                _unitsController.Add(unitType, count);
                _questsService.TrackUnitAdded(unitType, count, 1);
            }

            public void Add(Id unitType, UnitProgressionDto progression, int count)
            {
                _unitsController.Add(unitType, progression, count);
            }

            public void Spend(IUnitModel unitModel, int count)
            {
                _unitsController.Spend(unitModel, count);
                _questsService.TrackUnitSpend(unitModel.UnitType, count, unitModel.Stats.Level);
            }
            
            
            
            
            public IEnumerable<IUnitModel> GetUnits()
            {
                throw new System.NotImplementedException();
            }
            public bool TryGetUnit(Id typeUnit, UnitProgressionDto progression, out IUnitModel model)
            {
                throw new System.NotImplementedException();
            }
        }
        
        public class InventoryFacade : IInventoryController
        {
            private readonly IQuestInventory _questsService;
            private readonly IInventoryController _inventoryController;

            public InventoryFacade(IQuestInventory questsService, IInventoryController inventoryController)
            {
                _questsService = questsService;
                _inventoryController = inventoryController;
            }

            public void Add(Id item, int count)
            {
                _inventoryController.Add(item, count);
                _questsService.TrackInventoryAmountAdded(item, count);
            }
            public void Spend(Id item, int count)
            {
                _inventoryController.Spend(item, count);
                _questsService.TrackInventoryAmountSpend(item, count);
            }
            public void ExpandLimit(Id item, int count)
            {
                _inventoryController.ExpandLimit(item, count);
                _questsService.TrackInventoryExpandLimit(item, count);
            }
            
            
            
            public int GetCount(Id item)
            {
                throw new System.NotImplementedException();
            }
            public int GetLimit(Id item)
            {
                throw new System.NotImplementedException();
            }
            
            public IEnumerable<Id> GetItems()
            {
                throw new System.NotImplementedException();
            }

        }
    }
}