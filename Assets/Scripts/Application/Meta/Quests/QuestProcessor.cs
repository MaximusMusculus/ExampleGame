using System;

namespace Application.Meta.Quests
{
    public abstract class Quest
    {
        public abstract bool IsCompleted { get; }
        public abstract bool IsFinish { get; }
    }
    
    public class TestQuest : Quest, IDisposable
    {
        public override bool IsCompleted => false;
        public override bool IsFinish => false;

        public void Dispose()
        {
        }
    }
}