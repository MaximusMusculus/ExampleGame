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
                [TypeActionGroup.Inventory] = new QuestCountBasedProcessor(_config, data),
                [TypeActionGroup.Units] = new QuestActionProcessorUnitProcessor(_config, data)
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
    public class QuestCountBasedProcessor : IActionProcessor, IInventoryActionVisitor, IUnitActionVisitor
    {
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;
        private readonly QuestsDto _data;

        private IQuests _quests;
        private IQuestsController _questsController;

        public QuestCountBasedProcessor(Dictionary<Id, QuestCountBasedConfig> config, QuestsDto data)
        {
            _config = config;
            _data = data;
        }

        public void Process(IActionConfig actionConfig)
        {
            switch (actionConfig.ActionGroup)
            {
                case TypeActionGroup.Inventory:
                    ProcessInventory(actionConfig);
                    break;
                case TypeActionGroup.Units:
                    ProcessUnits(actionConfig);
                    break;
            }
        }

        private void ProcessInventory(IActionConfig actionConfig)
        {
            if (actionConfig is not IInventoryAction inventoryAction)
            {
                throw new ArgumentException("Action is not IInventoryAction info:" + actionConfig);
            }

            inventoryAction.Visit(this);
        }

        private void ProcessUnits(IActionConfig actionConfig)
        {
            if (actionConfig is not IUnitAction unitAction)
            {
                throw new ArgumentException("Action is not IUnitAction info:" + actionConfig);
            }

            unitAction.Visit(this);
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


        public void ItemAdd(InventoryActionConfig inventoryActionConfig)
        {
            throw new NotImplementedException();
            //обработка квестов на добавление предметов
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

        public void ItemSpend(InventoryActionConfig inventoryActionConfig)
        {
            throw new NotImplementedException();
        }

        public void ItemExpandLimit(InventoryActionConfig inventoryActionConfig)
        {
            throw new NotImplementedException();
        }

        public void UnitAdd(UnitActionConfig unitActionConfig)
        {
            throw new NotImplementedException();
        }

        public void UnitSpend(UnitActionConfig unitActionConfig)
        {
            throw new NotImplementedException();
        }
    }

    public class QuestActionProcessorUnitProcessor : ActionProcessorAbstract<UnitActionConfig>
    {
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;
        private readonly QuestsDto _data;

        public QuestActionProcessorUnitProcessor(Dictionary<Id, QuestCountBasedConfig> config, QuestsDto data)
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
}