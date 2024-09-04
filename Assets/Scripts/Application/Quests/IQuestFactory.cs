using System;

namespace Application.Meta.Quests
{
    public interface IQuestFactory
    {
        QuestData CreateData(QuestConfig config);
        QuestProcessor CreateProcessor(QuestConfig config, QuestData data);
    }
}