using System;

namespace Application.Meta.Quests.Imp
{
    public class QuestFactory : IQuestFactory
    {
        public QuestData CreateData(QuestConfig config)
        {
            throw new NotImplementedException();
        }

        public QuestProcessor CreateProcessor(QuestConfig config, QuestData data)
        {
            throw new NotImplementedException();
        }
    }
}