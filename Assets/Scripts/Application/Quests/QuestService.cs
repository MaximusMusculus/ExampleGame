namespace Application.Meta.Quests
{
    /// <summary>
    /// Внешнее апи для работы с квестами
    /// </summary>
    public interface IQuestService
    {
        void AddNewQuest(QuestConfig config);
    }
}