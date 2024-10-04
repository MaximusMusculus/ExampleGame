using System;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Models;
using UnityEngine;

namespace Meta.Controllers
{
    //2й способ (хуже кеш и скорость, но лучше расширение, чтение и понимание) Профит падает с количеством (до 100)
    public interface IQuestProcessorsFactory
    {
        QuestDto CreateData(IQuestConfig config);
        IQuestProcessor CreateController(IQuestConfig config, QuestDto dto);
    }

    public interface IQuestProcessor : IActionProcessor
    {
        string ActionGroup { get; }
    }

    public abstract class QuestCountBasedProcessor<TAction> : ActionProcessorAbstract<TAction>, IQuestProcessor
    {
        private readonly QuestCountBasedConfig _config;
        private readonly QuestCounterDto _data;
        public abstract string ActionGroup { get; }

        protected QuestCountBasedProcessor(QuestCountBasedConfig config, QuestCounterDto data)
        {
            _config = config;
            _data = data;
        }

        protected void ProcessQuest(TypeQuest typeQuest, Id item, int count)
        {
            if (!CheckTrigger(typeQuest) || !CheckTarget(item))
            {
                return;
            }

            var sumValue = _data.Value + count;
            _data.Value = Math.Clamp(sumValue, 0, _config.TargetValue);
            _data.IsCompleted = _data.Value >= _config.TargetValue;
            if (_data.IsCompleted)
            {
                Debug.Log("Quest completed:" + _config.QuestId);
            }
        }

        private bool CheckTrigger(TypeQuest triggerAction)
        {
            return _config.TriggerAction == triggerAction;
        }

        private bool CheckTarget(Id target)
        {
            return _config.TargetEntityId.Equals(target);
        }
    }

}

namespace Meta.Controllers.Imp
{
    public class QuestProcessorsProcessorsFactory : IQuestProcessorsFactory
    {
        private readonly IConditionProcessor _conditionProcessor;

        public QuestProcessorsProcessorsFactory(IConditionProcessor conditionProcessor)
        {
            _conditionProcessor = conditionProcessor;
        }

        public QuestDto CreateData(IQuestConfig config)
        {
            return config.TypeQuestGroup switch
            {
                TypeQuestGroup.CountBased => new QuestCounterDto(config.QuestId),
                TypeQuestGroup.Conditional => new QuestDto(config.QuestId),
                _ => throw new ArgumentException("Unknown quest type:" + config.TypeQuestGroup)
            };
        }

        public IQuestProcessor CreateController(IQuestConfig config, QuestDto data)
        {
            return config.TypeQuestGroup switch
            {
                TypeQuestGroup.CountBased => CreateQuestController((QuestCountBasedConfig) config, (QuestCounterDto) data),
                //TypeQuestGroup.Conditional => new QuestConditionalProcessor((QuestConditionalConfig) config, data, _conditionProcessor),
                _ => throw new ArgumentException("Unknown quest type:" + config.TypeQuestGroup)
            };
        }

        private IQuestProcessor CreateQuestController(QuestCountBasedConfig config, QuestCounterDto data)
        {
            return config.TriggerAction switch
            {
                TypeQuest.InventoryItemAdd => new QuestCountInventoryItemController(config, data),
                TypeQuest.InventoryItemSpend => new QuestCountInventoryItemController(config, data),
                TypeQuest.InventoryItemExpandLimit => new QuestCountInventoryItemController(config, data),
                TypeQuest.UnitAdd => new QuestCountUnitsController(config, data),
                TypeQuest.UnitSpend => new QuestCountUnitsController(config, data),
                _ => throw new ArgumentException("Unknown CountBased quest type:" + config.TriggerAction)
            };
        }
    }

    public class QuestCountUnitsController : QuestCountBasedProcessor<IUnitAction>, IUnitActionVisitor
    {
        public override string ActionGroup => TypeActionGroup.Units;
        public QuestCountUnitsController(QuestCountBasedConfig config, QuestCounterDto data) : base(config, data)
        {
        }

        protected override void Process(IUnitAction action)
        {
            action.Visit(this);
        }


        public void UnitAdd(Id typeUnit, UnitProgressionDto progression, int count)
        {
            ProcessQuest(TypeQuest.UnitAdd, typeUnit, count);
        }

        public void UnitSpend(Id typeUnit, UnitProgressionDto progression, int count)
        {
            ProcessQuest(TypeQuest.UnitSpend, typeUnit, count);
        }


    }

    public class QuestCountInventoryItemController : QuestCountBasedProcessor<IInventoryAction>, IInventoryActionVisitor
    {
        public override string ActionGroup => TypeActionGroup.Inventory;

        public QuestCountInventoryItemController(QuestCountBasedConfig config, QuestCounterDto data) : base(config, data)
        {
        }

        protected override void Process(IInventoryAction action)
        {
            action.Visit(this);
        }

        public void ItemAdd(Id itemId, int count)
        {
            ProcessQuest(TypeQuest.InventoryItemAdd, itemId, count);
        }

        public void ItemSpend(Id itemId, int count)
        {
            ProcessQuest(TypeQuest.InventoryItemSpend, itemId, count);
        }

        public void ItemExpandLimit(Id itemId, int count)
        {
            ProcessQuest(TypeQuest.InventoryItemExpandLimit, itemId, count);
        }
    }
}