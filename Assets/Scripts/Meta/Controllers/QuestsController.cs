using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta.Controllers
{
    public interface IQuests
    {
        public IEnumerable<IQuest> GetAll();
    }
    public interface IQuestsController
    {
        void AddNewQuest(Id configId);
        void ClaimReward(IQuest quest);
        void Remove(IQuest quest);
    }

    
    public class QuestsController : IActionProcessor, IQuestsController, IQuests
    {
        private readonly List<IQuestProcessor> _metaQuestControllers = new List<IQuestProcessor>(ConstDefaultCapacity.Small);

        private readonly IQuestControllerFactory _questsControllerFactory;
        private readonly Dictionary<Id, IQuestConfig> _configs;
        private readonly QuestCollectionDto _questData;
        private readonly IActionProcessor _rewardProcessor;

        public QuestsController(QuestCollectionConfig questConfig,QuestCollectionDto questData,
            IQuestControllerFactory questsControllerFactory, IActionProcessor rewardProcessor)
        {
            _questData = questData;
            _rewardProcessor = rewardProcessor;
            _questsControllerFactory = questsControllerFactory;

            var count = questConfig.GetAll().Count();
            _configs = new Dictionary<Id, IQuestConfig>(count);
            foreach (var config in questConfig.GetAll())
            {
                _configs.Add(config.QuestId, config);
            }

            foreach (var questDto in _questData.GetAll())
            {
                var config = _configs[questDto.ConfigId];
                var questEntity = _questsControllerFactory.CreateController(config, questDto);
                _metaQuestControllers.Add(questEntity);
            }
        }

        public void AddNewQuest(Id questId)
        {
            var questConfig = _configs[questId];
            var questDto = _questsControllerFactory.CreateData(questConfig);
            _questData.Add(questDto);
            var questActionController = _questsControllerFactory.CreateController(questConfig, questDto);
            _metaQuestControllers.Add(questActionController);
        }

        
        public void ClaimReward(IQuest quest)
        {
            ClaimReward(quest.Id);
        }

        
        public void ClaimReward(Id questId)
        {
            var quest = _questData.GetAll().FirstOrDefault(c => c.Id.Equals(questId));
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
            foreach (var questController in _metaQuestControllers)
            {
                questController.Process(actionConfig);
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