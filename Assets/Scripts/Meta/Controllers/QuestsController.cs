using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta.Controllers
{
    public interface IQuests
    {
        public IEnumerable<IQuest> GetAll();
    }

    public interface IQuestsController : IQuests
    {
        void AddNewQuest(Id configId);
        void ClaimReward(IQuest quest);
        void Remove(IQuest quest);
    } 
}

namespace Meta.Controllers.Imp
{
    /// <summary>
    /// процессит все квесты на количества инвентаря
    /// </summary>
    public class InventoryQuestsProcessor : ActionProcessorAbstract<IInventoryAction>, IInventoryActionVisitor
    {
        private QuestCollectionConfig _questConfig;
        private QuestCollectionDto _questData;

        public InventoryQuestsProcessor(QuestCollectionConfig questConfig, QuestCollectionDto questData)
        {
            _questConfig = questConfig;
            _questData = questData;
        }

        protected override void Process(IInventoryAction action)
        {
            throw new NotImplementedException();
        }


        public void ItemAdd(Id itemId, int count)
        {
            throw new NotImplementedException();
        }

        public void ItemSpend(Id itemId, int count)
        {
            throw new NotImplementedException();
        }

        public void ItemExpandLimit(Id itemId, int count)
        {
            throw new NotImplementedException();
        }
    }
    
    public class QuestsController : IActionProcessor, IQuestsController, IQuests, IActionVisitor
    {
        private readonly List<IQuestProcessor> _metaQuestControllers = new List<IQuestProcessor>(ConstDefaultCapacity.Small);

        private readonly IQuestProcessorsFactory _questsProcessorsFactory;
        private readonly Dictionary<Id, IQuestConfig> _configs;
        private readonly QuestCollectionDto _questData;
        private readonly IActionProcessor _rewardProcessor;

        public QuestsController(QuestCollectionConfig questConfig, QuestCollectionDto questData,
            IQuestProcessorsFactory questsProcessorsFactory, IActionProcessor rewardProcessor)
        {
            _questData = questData;
            _rewardProcessor = rewardProcessor;
            _questsProcessorsFactory = questsProcessorsFactory;

            var count = questConfig.GetAll().Count();
            _configs = new Dictionary<Id, IQuestConfig>(count);
            foreach (var config in questConfig.GetAll())
            {
                _configs.Add(config.QuestId, config);
            }

            foreach (var questDto in _questData.GetAll())
            {
                var config = _configs[questDto.ConfigId];
                var questEntity = _questsProcessorsFactory.CreateController(config, questDto);
                _metaQuestControllers.Add(questEntity);
            }
        }

        public void AddNewQuest(Id questId)
        {
            var questConfig = _configs[questId];
            var questDto = _questsProcessorsFactory.CreateData(questConfig);
            _questData.Add(questDto);
            var questActionController = _questsProcessorsFactory.CreateController(questConfig, questDto);
            _metaQuestControllers.Add(questActionController);
        }


        public void ClaimReward(IQuest quest)
        {
            ClaimReward(quest.ConfigId);
        }
        private void ClaimReward(Id questId)
        {
            var quest = _questData.GetAll().FirstOrDefault(c => c.ConfigId.Equals(questId));
            Assert.IsNotNull(quest);
            Assert.IsTrue(quest.IsCompleted);
            Assert.IsFalse(quest.IsRewarded);

            var questConfig = _configs[quest.ConfigId];
            _rewardProcessor.Process(questConfig.Reward);
            quest.IsRewarded = true;
        }
        
        public void Remove(IQuest quest)
        {
            throw new NotImplementedException();
        }


        //это должно быть не здесь. 
        public void Process(IActionConfig actionConfig)
        {
            actionConfig.Accept(this);
        }
        
        public void Visit(IInventoryAction inventoryActionConfig)
        {
            foreach (var questController in _metaQuestControllers)
            {
                if (questController is IInventoryActionVisitor)
                {
                    inventoryActionConfig.Visit((IInventoryActionVisitor)questController);
                }
            }
        }
        public void Visit(IUnitAction unitActionConfig)
        {
            foreach (var questController in _metaQuestControllers)
            {
                if (questController is IUnitActionVisitor)
                {
                    unitActionConfig.Visit((IUnitActionVisitor)questController);
                }
            }
        }

        


        public void ProcessBattleEvent()
        {
            /*foreach (var questController in _battleQuestControllers)
            {
                questController.ProcessBattleEvent();                
            }*/
        }

        public IEnumerable<IQuest> GetAll()
        {
            return _questData.GetAll();
        }


    }
}