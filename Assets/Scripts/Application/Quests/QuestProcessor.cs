using System;

namespace Application.Meta.Quests
{
    public abstract class QuestProcessor
    {
    }
    
    public class TestQuestProcessor : QuestProcessor, IDisposable
    {
        public void Dispose()
        {
            //do nothing
        }
    }
}