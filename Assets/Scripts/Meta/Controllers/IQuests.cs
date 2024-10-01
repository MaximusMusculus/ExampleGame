using System;
using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Models;

namespace Meta.Controllers
{
    public interface IQuestsCollectionDto<TQuestType> where TQuestType: IQuest
    {
        bool TryGet(TypeQuest typeQuest, out HashSet<TQuestType> quest);
    }
    public interface IQuestConfigCollection<TQuestConfig> where  TQuestConfig : IQuestConfig
    {
        bool TryGet(Id id, out TQuestConfig questConfig);
    }

    //по задумке - процессит список CountBased
    public abstract class QuestsCountBasedProcessorAbstract<TAction> : ActionProcessorAbstract<TAction>
    {
        private readonly IQuestsCollectionDto<QuestCounterDto> _dataCollection;
        private readonly IQuestConfigCollection<QuestCountBasedConfig> _configCollection;
        
        protected QuestsCountBasedProcessorAbstract(IQuestsCollectionDto<QuestCounterDto> dataCollection, IQuestConfigCollection<QuestCountBasedConfig> configCollection)
        {
            _dataCollection = dataCollection;
            _configCollection = configCollection;
        }
        
        protected abstract void ProcessCountQuest(TypeQuest typeTrigger, Id item, int count);
        protected abstract void UpdateProgress(QuestCounterDto data, QuestCountBasedConfig config, int count);
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