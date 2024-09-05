using System;
using System.Collections.Generic;

namespace Application.Meta.Quests.Imp
{
    public class QuestInfo
    {
        public List<QuestConfig> Configs = new List<QuestConfig>();
        public List<QuestData> Data = new List<QuestData>();
    }
    
    public class QuestService : IQuestService, IDisposable
    {
        private readonly List<QuestData> _data;
        private readonly List<QuestConfig> _configs;
        private readonly List<Quest> _quests;
        private readonly IQuestFactory _questFactory;
        
        public QuestService(QuestInfo questInfo, IQuestFactory questFactory)
        {
            _data = questInfo.Data;
            _configs = questInfo.Configs;
            _quests = new List<Quest>();

            for (var i = 0; i < _data.Count; i++)
            {
                _quests.Add(questFactory.CreateProcessor(_configs[i], _data[i]));
            }
        }

        public void Dispose()
        {
            foreach (var processor in _quests)
            {
                if (processor is IDisposable disposableProcessor)
                {
                    disposableProcessor.Dispose();
                }
            }
            _quests.Clear();
        }
        
        // public ID AddNewQuest(QuestConfig config) //or ConfigID
        public void AddNewQuest(QuestConfig config)
        {
            var data = _questFactory.CreateData(config);
            _data.Add(data);
            _configs.Add(config);
            var processor = _questFactory.CreateProcessor(config, data);
            _quests.Add(processor);
        }
        
        public void FinishQuest(Quest quest)
        {
            var index = _quests.IndexOf(quest);
            if (index == -1)
            {
                return;
            }
            _data.RemoveAt(index);
            _configs.RemoveAt(index);
            
            if (quest is IDisposable disposableProcessor)
            {
                disposableProcessor.Dispose();
            }
            _quests.RemoveAt(index);
        }
        
        public IEnumerator<Quest> Quests => _quests.GetEnumerator();
    }
}