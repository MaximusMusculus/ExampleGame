using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestMetaSimple
    {
        
        private List<ItemDto> _itemsData;
        private List<UnitDto> _unitsData;
        
        private InventoryController _inventory;
        private UnitsController _units;


        [SetUp]
        public void SetUp()
        {
            _itemsData = new List<ItemDto>();
            _unitsData = new List<UnitDto>();

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
                //Reward = new UnitActionConfig {MetaAction = TypeMetaAction.UnitAdd, TypeUnit = MapTestId.UnitAssault.Id(), Count = 1}
            });

        }
        
        public interface INotifyService
        {
            void Notify(string message);
        }

        public class QuestControllerFacade : IQuestsController
        {
            private readonly INotifyService _notifyService;
            private readonly IQuestsController _questsController;

            public QuestControllerFacade(IQuestsController questsController, INotifyService notifyService)
            {
                _questsController = questsController;
                _notifyService = notifyService;
            }

            public void AddNewQuest(Id configId)
            {
               // _questsController.AddNewQuest(configId);
                _notifyService.Notify("New quest added");
            }

            public void ClaimReward(Id id)
            {
                //_questsController.ClaimReward(id);
                _notifyService.Notify("Reward claimed");
            }

            public void AddNewQuest(IQuestConfig configId)
            {
                throw new System.NotImplementedException();
            }

            public void ClaimReward(IQuest quest)
            {
                throw new System.NotImplementedException();
            }

            public void Remove(IQuest quest)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerable<IQuest> GetAll()
            {
                throw new System.NotImplementedException();
            }
        }

    }
}