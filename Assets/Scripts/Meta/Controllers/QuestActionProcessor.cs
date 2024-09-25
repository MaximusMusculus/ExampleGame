using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Models;

namespace Meta.Controllers
{
    //стратегия пресозданных процессоров.
    
    public class QuestMetaConditionalProcessor : IActionProcessor
    {
        private readonly Dictionary<Id, QuestConditionalConfig> _config;
        private readonly IConditionProcessor _conditionProcessor;
        private readonly List<QuestDto> _data;

        public QuestMetaConditionalProcessor(List<QuestDto> data, List<QuestConditionalConfig> questConfigs, IConditionProcessor conditionProcessor)
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
            foreach (var questDto in _data)
            {
                var config = _config[questDto.QuestId];
                if (!config.Triggers.Contains(actionConfig.TypeMetaAction))
                {
                    continue;
                }
                var isCompleted = _conditionProcessor.Check(config.Condition);
                questDto.IsCompleted = isCompleted;
            }
        }
    }
    
    public class QuestMetaCountBasedProcessor : IActionProcessor
    {
        private readonly Dictionary<TypeMetaAction, IActionProcessor> _questProcessors;
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;

        public QuestMetaCountBasedProcessor(List<QuestCountBasedConfig> questConfigs, List<QuestCounterDto> data)
        {
             _config = new Dictionary<Id, QuestCountBasedConfig>(questConfigs.Count);
            foreach (var questConfig in questConfigs)
            {
                _config.Add(questConfig.QuestId, questConfig);
            }
            
            var itemQuestProcessor = new QuestActionItemProcessor(_config, data);
            var unitQuestProcessor = new QuestActionUnitProcessor(_config, data);
            
            _questProcessors = new Dictionary<TypeMetaAction, IActionProcessor>();
            _questProcessors[TypeMetaAction.InventoryItemAdd] = itemQuestProcessor;
            _questProcessors[TypeMetaAction.InventoryItemSpend] = itemQuestProcessor;
            _questProcessors[TypeMetaAction.InventoryItemExpandLimit] = itemQuestProcessor;

            _questProcessors[TypeMetaAction.UnitAdd] = unitQuestProcessor;
            _questProcessors[TypeMetaAction.UnitSpend] = unitQuestProcessor;
        }

        public void Process(IActionConfig actionConfig)
        {
            if (_questProcessors.TryGetValue(actionConfig.TypeMetaAction, out var action) == false)
            {
                throw new ArgumentException($"Action {actionConfig.TypeMetaAction} not found");
            }
            action.Process(actionConfig);
        }
    }
    
    public class QuestActionItemProcessor : ActionAbstract<ItemActionConfig>
    {
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;
        private readonly List<QuestCounterDto> _data;

        public QuestActionItemProcessor(Dictionary<Id, QuestCountBasedConfig> config, List<QuestCounterDto> data)
        {
            _config = config;
            _data = data;
        }

        protected override void Process(ItemActionConfig args)
        {
            foreach (var questCounter in _data)
            {
                var config = _config[questCounter.QuestId];
                if (config.TargetAction.Equals(args.MetaAction) && config.TargetEntityId.Equals(args.TypeItem))
                {
                    var sumValue = questCounter.Value + args.Count;
                    questCounter.Value = Math.Clamp(sumValue, 0, config.TargetValue);
                    questCounter.IsCompleted = questCounter.Value >= config.TargetValue;
                }
            }
        }
    }
    public class QuestActionUnitProcessor : ActionAbstract<UnitActionConfig>
    {
        private readonly Dictionary<Id, QuestCountBasedConfig> _config;
        private readonly List<QuestCounterDto> _data;

        public QuestActionUnitProcessor(Dictionary<Id, QuestCountBasedConfig> config, List<QuestCounterDto> data)
        {
            _config = config;
            _data = data;
        }

        protected override void Process(UnitActionConfig args)
        {
            foreach (var questCounter in _data)
            {
                var config = _config[questCounter.QuestId];
                if (config.TargetAction.Equals(args.MetaAction) && config.TargetEntityId.Equals(args.TypeUnit))
                {
                    var sumValue = questCounter.Value + args.Count;
                    questCounter.Value = Math.Clamp(sumValue, 0, config.TargetValue);
                    questCounter.IsCompleted = questCounter.Value >= config.TargetValue;
                }
            }
        }
    }
    
    
    //2й способ
    public interface IQuestControllerFactory
    {
        QuestDto CreateData(IQuestConfig config);
        IActionProcessor CreateController(IQuestConfig config, QuestDto dto);
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
                TypeQuest.CountBased => new QuestCounterDto {QuestId = config.QuestId},
                TypeQuest.Conditional => new QuestDto {QuestId = config.QuestId},
                _ => throw new ArgumentException("Unknown quest type:" + config.TypeQuest)
            };
        }

        public IActionProcessor CreateController(IQuestConfig config, QuestDto data)
        {
            return config.TypeQuest switch
            {
                TypeQuest.CountBased => CreateQuestController((QuestCountBasedConfig) config, (QuestCounterDto) data),
                TypeQuest.Conditional => new QuestConditionalController((QuestConditionalConfig) config, data, _conditionProcessor),
                _ => throw new ArgumentException("Unknown quest type:" + config.TypeQuest)
            };
        }

        private IActionProcessor CreateQuestController(QuestCountBasedConfig config, QuestCounterDto data)
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
    
    
    public class QuestsController : IActionProcessor
    {
        private readonly List<IActionProcessor> _questControllers = new List<IActionProcessor>(ConstDefaultCapacity.Small);
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
                var config = _configs[questDto.QuestId];
                var questEntity = _questsControllerFactory.CreateController(config, questDto);
                _questControllers.Add(questEntity);
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
            _questControllers.Add(questActionController);
        }

        public void ClaimReward()//quest id?
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
            foreach (var questController in _questControllers)
            {
                questController.Process(actionConfig);
            }
        }
    }

    public class QuestCountBasedItemController : ActionAbstract<ItemActionConfig>
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
    public class QuestCountBasedUnitController : ActionAbstract<UnitActionConfig>
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
    public class QuestConditionalController : IActionProcessor
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