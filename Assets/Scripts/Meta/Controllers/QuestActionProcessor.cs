using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers.Actions;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta.Controllers
{
    //1й способ (лучше скорость и кеш, но хуже контроль и расширение) Профит тем выше ,чем больше квестов. 100++
    public class QuestMetaConditionalProcessor : IActionProcessor
    {
        private readonly Dictionary<Id, QuestConditionalConfig> _config;
        private readonly IConditionProcessor _conditionProcessor;
        private readonly QuestsDto _data;

        public QuestMetaConditionalProcessor(List<QuestConditionalConfig> questConfigs, QuestsDto data, IConditionProcessor conditionProcessor)
        {
            _data = data;
            _conditionProcessor = conditionProcessor;
            _config = new Dictionary<Id, QuestConditionalConfig>(questConfigs.Count);
            foreach (var questConfig in questConfigs)
            {
                _config[questConfig.QuestId] = questConfig;
            }
        }

        public void Process(IActionConfig actionConfig)
        {
            foreach (var questDto in _data.Quests)
            {
                if (_config.TryGetValue(questDto.ConfigId, out var config) == false)
                {
                    continue;
                }

                throw new NotImplementedException();
                /*if (!config.Triggers.Contains(actionConfig.TypeMetaAction))
                {
                    continue;
                }*/

                var isCompleted = _conditionProcessor.Check(config.Condition);
                questDto.IsCompleted = isCompleted;
            }
        }
    }

    public class QuestMetaCountBasedProcessor : IActionProcessor
    {
        private readonly Dictionary<string, IActionProcessor> _questProcessors;
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;

        public QuestMetaCountBasedProcessor(List<QuestCountBasedConfig> questConfigs, QuestsDto data)
        {
            _config = new Dictionary<Id, QuestCountBasedConfig>(questConfigs.Count);
            foreach (var questConfig in questConfigs)
            {
                _config.Add(questConfig.QuestId, questConfig);
            }
            
            _questProcessors = new Dictionary<string, IActionProcessor>
            {
                [TypeActionGroup.Collection] = new ActionCollectionProcessor(this),
                [TypeActionGroup.Inventory] = new QuestInventoryProcessor(_config, data),
                [TypeActionGroup.Units] = new QuestUnitProcessor(_config, data)
            };
        }

        public void Process(IActionConfig actionConfig)
        {
            if (_questProcessors.TryGetValue(actionConfig.ActionGroup, out var action) == false)
            {
                throw new ArgumentException($"Action {actionConfig.ActionGroup} not found");
            }

            action.Process(actionConfig);
        }
    }

    //--
    public class QuestInventoryProcessor : ActionProcessorAbstract<IInventoryAction>, IInventoryActionVisitor
    {
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;
        private readonly QuestsDto _data;

        private IQuests _quests;
        private IQuestsController _questsController;

        public QuestInventoryProcessor(Dictionary<Id, QuestCountBasedConfig> config, QuestsDto data)
        {
            _config = config;
            _data = data;
        }
        
        /*protected override void Process(ItemActionConfig args)
        {
            args.Visit(this);
            return;

            foreach (var questDto in _data.Quests)
            {
                if (_config.TryGetValue(questDto.ConfigId, out var config) == false)
                {
                    continue;
                }


                /*if (config.TriggerAction.Equals(args.MetaAction) && config.TargetEntityId.Equals(args.TypeItem))
                {
                    if(_data.Counters.TryGetValue(questDto.Id, out var count) == false)
                    {
                        _data.Counters[questDto.Id] = 0;
                    }
                    
                    _data.Counters[questDto.Id] += args.Count;
                    _data.Counters[questDto.Id] = Math.Clamp(_data.Counters[questDto.Id], 0, config.TargetValue);
                    questDto.IsCompleted = _data.Counters[questDto.Id] >= config.TargetValue;
                }#1#
            }
        }*/

        protected override void Process(IInventoryAction action)
        {
            action.Visit(this);
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

    public class QuestUnitProcessor : ActionProcessorAbstract<UnitActionConfig>
    {
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;
        private readonly QuestsDto _data;

        public QuestUnitProcessor(Dictionary<Id, QuestCountBasedConfig> config, QuestsDto data)
        {
            _config = config;
            _data = data;
        }

        protected override void Process(UnitActionConfig action)
        {
            foreach (var questCounterDto in _data.Quests)
            {
                if (_config.TryGetValue(questCounterDto.ConfigId, out var config) == false)
                {
                    continue;
                }

                throw new NotImplementedException();

                /*if (config.TriggerAction.Equals(args.MetaAction) && config.TargetEntityId.Equals(args.TypeUnit))
                {
                    if(_data.Counters.TryGetValue(questCounterDto.Id, out var count) == false)
                    {
                        _data.Counters[questCounterDto.Id] = 0;
                    }
                    
                    _data.Counters[questCounterDto.Id] += args.Count;
                    _data.Counters[questCounterDto.Id] = Math.Clamp(_data.Counters[questCounterDto.Id], 0, config.TargetValue);
                    questCounterDto.IsCompleted = _data.Counters[questCounterDto.Id] >= config.TargetValue;
                }*/
            }
        }
    }

    
    
    
    

    /// <summary>
    /// Квест контроллер. Оперирует квестами:
    /// Создает, удаляет и выдает награду  
    /// </summary>
    public class QuestController : IQuestsController, IQuests
    {
        private readonly QuestCollectionConfig _config;
        private readonly QuestsDto _quests;
        private readonly IIdProvider _idProvider;
        private readonly IActionProcessor _actionProcessor;

        private readonly Dictionary<Id, QuestDto> _questsHash;

        public QuestController(QuestCollectionConfig config, QuestsDto quests, IIdProvider idProvider, IActionProcessor actionProcessor)
        {
            _config = config;
            _quests = quests;
            _idProvider = idProvider;
            _actionProcessor = actionProcessor;

            _questsHash = new Dictionary<Id, QuestDto>(quests.Quests.Count + ConstDefaultCapacity.Small);
            foreach (var questDto in _quests.Quests)
            {
                _questsHash.Add(questDto.Id, questDto);
            }
        }

        public void AddNewQuest(Id configId)
        {
            var questData = new QuestDto(configId, _idProvider.GetId());
            _questsHash.Add(questData.Id, questData);
            _quests.Add(questData);
        }

        public void ClaimReward(IQuest questElem)
        {
            var quest = _quests.Quests.FirstOrDefault(c => c.Id.Equals(questElem.Id));
            Assert.IsNotNull(quest);
            Assert.IsTrue(quest.IsCompleted);
            Assert.IsFalse(quest.IsRewarded);

            var questConfig = _config.GetAll().First(s => s.QuestId.Equals(quest.ConfigId));
            _actionProcessor.Process(questConfig.Reward);
            quest.IsRewarded = true;
        }

        public void RemoveQuest(Id questId)
        {
            _questsHash.TryGetValue(questId, out var quest);
            Assert.IsNotNull(quest);
            _quests.Remove(quest);
        }

        public void Remove(IQuest questElem)
        {
            _questsHash.TryGetValue(questElem.Id, out var quest);
            Assert.IsNotNull(quest);
            _quests.Remove(quest);
        }

        //-- 
        public IEnumerable<IQuest> GetAll()
        {
            return _quests.Quests;
        }

        public bool TryGet(Id id, out IQuest quest)
        {
            var isTryGet = _questsHash.TryGetValue(id, out var questDto);
            quest = questDto;
            return isTryGet;
        }

        public bool TryGetCount(Id id, out int count) //Quest?
        {
            return _quests.Counters.TryGetValue(id, out count);
        }
    }
    
    
     public interface IQuestsCollectionDto<TQuestType> where TQuestType: IQuest
    {
        bool TryGet(TypeQuest typeQuest, out HashSet<TQuestType> quest);
    }
    public interface IQuestConfigCollection<TQuestConfig> where  TQuestConfig : IQuestConfig
    {
        bool TryGet(Id id, out TQuestConfig questConfig);
    }
    

    public class QuestCountInventoryProcessor : ActionProcessorAbstract<IInventoryAction>, IInventoryActionVisitor
    {
        private readonly IQuestsCollectionDto<QuestCounterDto> _dataCollection;
        private readonly IQuestConfigCollection<QuestCountBasedConfig> _configCollection;
        
        public QuestCountInventoryProcessor(IQuestsCollectionDto<QuestCounterDto> dataCollection, IQuestConfigCollection<QuestCountBasedConfig> configCollection)
        {
            _dataCollection = dataCollection;
            _configCollection = configCollection;
        }
        
        protected override void Process(IInventoryAction action)
        {
            action.Visit(this);
        }

        public void ItemAdd(Id itemId, int count)
        {
            ProcessQuests(TypeQuest.InventoryItemAdd, itemId, count);
        }
        public void ItemSpend(Id itemId, int count)
        {
            ProcessQuests(TypeQuest.UnitSpend, itemId, count);
        }
        public void ItemExpandLimit(Id itemId, int count)
        {
            ProcessQuests(TypeQuest.InventoryItemExpandLimit, itemId, count);
        }

        private void ProcessQuests(TypeQuest typeTrigger, Id item, int count)
        {
            if (_dataCollection.TryGet(typeTrigger, out var questSet) == false)
            {
                return;
            }

            foreach (var quest in questSet)
            {
                _configCollection.TryGet(quest.ConfigId, out var questConfig);
                if (!CheckTarget(questConfig, item))
                {
                    continue;
                }

                UpdateProgress(quest, questConfig, count);
            }
        }

        private bool CheckTarget(QuestCountBasedConfig config, Id target)
        {
            return config.TargetEntityId.Equals(target);
        }
        
        private void UpdateProgress(QuestCounterDto data, QuestCountBasedConfig config, int count)
        {
            var sumValue = data.Value + count;
            data.Value = Math.Clamp(sumValue, 0, config.TargetValue);
            data.IsCompleted = data.Value >= config.TargetValue;
        }
        
    }
}