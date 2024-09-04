using System;
using System.Collections.Generic;

namespace Application.Meta.Quests
{
    public class QuestServiceImp : IQuestService, IDisposable
    {
        private readonly List<QuestData> _data;
        private readonly List<QuestConfig> _configs;
        private readonly List<QuestProcessor> _questProcessors;

        private readonly IQuestFactory _questFactory;

        public QuestServiceImp(List<QuestConfig> configs, List<QuestData> data, IQuestFactory questFactory)
        {
            _data = data;
            _configs = configs;
            _questProcessors = new List<QuestProcessor>();

            for (var i = 0; i < data.Count; i++)
            {
                _questProcessors.Add(questFactory.CreateProcessor(configs[i], data[i]));
            }
        }

        public void Dispose()
        {
            foreach (var processor in _questProcessors)
            {
                if (processor is IDisposable disposableProcessor)
                {
                    disposableProcessor.Dispose();
                }
            }
            _questProcessors.Clear();
        }
        
        public void AddNewQuest(QuestConfig config)
        {
            var data = _questFactory.CreateData(config);
            _data.Add(data);
            _configs.Add(config);
            _questProcessors.Add(_questFactory.CreateProcessor(config, data));
        }
    }
}