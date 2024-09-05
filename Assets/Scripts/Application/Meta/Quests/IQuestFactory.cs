namespace Application.Meta.Quests
{
    public interface IQuestFactory
    {
        QuestData CreateData(QuestConfig config);
        Quest CreateProcessor(QuestConfig config, QuestData data);
    }
}