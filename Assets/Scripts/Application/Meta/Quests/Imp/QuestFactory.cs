using System;

namespace Application.Meta.Quests.Imp
{
    public class QuestFactory : IQuestFactory
    {
        //IIdProvider
        public QuestData CreateData(QuestConfig config)
        {
            throw new NotImplementedException();
            //var id = IdProvider.GetId();
            //data.Id = id; (or constructor)
        }

        public Quest CreateProcessor(QuestConfig config, QuestData data)
        {
            throw new NotImplementedException();
        }
    }
}