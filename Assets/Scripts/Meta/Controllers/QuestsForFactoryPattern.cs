﻿using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Models;

namespace Meta.Controllers
{
    //2й способ (хуже кеш и скорость, но лучше расширение, чтение и понимание) Профит падает с количеством (до 100)
    public interface IQuestControllerFactory
    {
        QuestDto CreateData(IQuestConfig config);
        IQuestController CreateController(IQuestConfig config, QuestDto dto);
    }

    public class QuestControllerControllerFactory : IQuestControllerFactory
    {
        private readonly IConditionProcessor _conditionProcessor;

        public QuestControllerControllerFactory(IConditionProcessor conditionProcessor)
        {
            _conditionProcessor = conditionProcessor;
        }

        public QuestDto CreateData(IQuestConfig config)
        {
            return config.TypeQuest switch
            {
                TypeQuest.CountBased => new QuestCounterDto {ConfigId = config.QuestId},
                TypeQuest.Conditional => new QuestDto {ConfigId = config.QuestId},
                _ => throw new ArgumentException("Unknown quest type:" + config.TypeQuest)
            };
        }

        public IQuestController CreateController(IQuestConfig config, QuestDto data)
        {
            return config.TypeQuest switch
            {
                TypeQuest.CountBased => CreateQuestController((QuestCountBasedConfig) config, (QuestCounterDto) data),
                TypeQuest.Conditional => new QuestConditionalController((QuestConditionalConfig) config, data, _conditionProcessor),
                _ => throw new ArgumentException("Unknown quest type:" + config.TypeQuest)
            };
        }

        private IQuestController CreateQuestController(QuestCountBasedConfig config, QuestCounterDto data)
        {
            return config.TargetAction switch
            {
                TypeMetaAction.InventoryItemAdd => new QuestCountBasedItemController(config, data),
                TypeMetaAction.InventoryItemSpend => new QuestCountBasedItemController(config, data),
                TypeMetaAction.InventoryItemExpandLimit => new QuestCountBasedItemController(config, data),
                TypeMetaAction.UnitAdd => new QuestCountBasedUnitController(config, data),
                TypeMetaAction.UnitSpend => new QuestCountBasedUnitController(config, data),
                _ => throw new ArgumentException("Unknown CountBased quest type:" + config.TargetAction)
            };
        }
    }


    public interface IQuestController : IActionProcessor
    {
    }

    public class QuestsController : IActionProcessor
    {
        private readonly List<IQuestController> _metaQuestControllers = new List<IQuestController>(ConstDefaultCapacity.Small);

        private readonly IQuestControllerFactory _questsControllerFactory;
        private readonly Dictionary<Id, IQuestConfig> _configs;
        private readonly QuestCollectionDto _questData;

        public QuestsController(QuestCollectionDto questData, QuestCollectionConfig questConfig, IQuestControllerFactory questsControllerFactory)
        {
            _questData = questData;
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
            AddNewQuest(_configs[questId]);
        }

        public void AddNewQuest(IQuestConfig questConfig)
        {
            var questDto = _questsControllerFactory.CreateData(questConfig);
            _questData.Add(questDto);
            var questActionController = _questsControllerFactory.CreateController(questConfig, questDto);
            _metaQuestControllers.Add(questActionController);
        }

        public void ClaimReward() //quest id?
        {

        }

        public void RemoveQuest()
        {
            //_questData.Remove()
        }



        /// <summary>
        /// Process ActionBasedQuests
        /// </summary>
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
    }

    public class QuestCountBasedItemController : ActionAbstract<ItemActionConfig>, IQuestController
    {
        private readonly QuestCountBasedConfig _config;
        private readonly QuestCounterDto _data;

        public QuestCountBasedItemController(QuestCountBasedConfig config, QuestCounterDto data)
        {
            _config = config;
            _data = data;
        }

        protected override void Process(ItemActionConfig args)
        {
            if (_config.TargetAction.Equals(args.MetaAction) && _config.TargetEntityId.Equals(args.TypeItem))
            {
                var sumValue = _data.Value + args.Count;
                _data.Value = Math.Clamp(sumValue, 0, _config.TargetValue);
                _data.IsCompleted = _data.Value >= _config.TargetValue;
            }
        }
    }

    public class QuestCountBasedUnitController : ActionAbstract<UnitActionConfig>, IQuestController
    {
        private readonly QuestCountBasedConfig _config;
        private readonly QuestCounterDto _data;

        public QuestCountBasedUnitController(QuestCountBasedConfig config, QuestCounterDto data)
        {
            _config = config;
            _data = data;
        }

        protected override void Process(UnitActionConfig args)
        {
            if (_config.TargetAction.Equals(args.MetaAction) && _config.TargetEntityId.Equals(args.TypeUnit))
            {
                var sumValue = _data.Value + args.Count;
                _data.Value = Math.Clamp(sumValue, 0, _config.TargetValue);
                _data.IsCompleted = _data.Value >= _config.TargetValue;
            }
        }
    }

    public class QuestConditionalController : IQuestController
    {
        private readonly QuestConditionalConfig _config;
        private readonly QuestDto _data;
        private readonly IConditionProcessor _conditionProcessor;

        public QuestConditionalController(QuestConditionalConfig config, QuestDto data, IConditionProcessor conditionProcessor)
        {
            _config = config;
            _data = data;
            _conditionProcessor = conditionProcessor;
        }

        public void Process(IActionConfig actionConfig)
        {
            if (_config.Triggers.Contains(actionConfig.TypeMetaAction))
            {
                var isCompleted = _conditionProcessor.Check(_config.Condition);
                _data.IsCompleted = isCompleted;
            }
        }
    }
}