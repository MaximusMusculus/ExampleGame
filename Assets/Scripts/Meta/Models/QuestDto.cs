using System.Collections.Generic;
using AppRen;

namespace Meta.Models
{
    public interface IQuest
    {
        public Id ConfigId { get; }
        public bool IsCompleted { get; }
        public bool IsRewarded { get; }
    }

    public class QuestDto : IQuest
    {
        public Id ConfigId { get; set; }

        public QuestDto(Id configId)
        {
            ConfigId = configId;
        }

        public bool IsCompleted { get; set; }
        public bool IsRewarded { get; set; }
    }


    public class QuestCounterDto : QuestDto
    {
        public int Value;

        public QuestCounterDto(Id configId) : base(configId)
        {
        }
    }

    public class QuestCollectionDto
    {
        public List<QuestDto> ConditionalQuest = new List<QuestDto>();
        public List<QuestCounterDto> CountBasedQuest = new List<QuestCounterDto>();

        public IEnumerable<QuestDto> GetAll()
        {
            foreach (var questCounterDto in CountBasedQuest)
            {
                yield return questCounterDto;
            }

            foreach (var questDto in ConditionalQuest)
            {
                yield return questDto;
            }
        }

        public void Add(IQuest questDto)
        {
            if (questDto is QuestCounterDto counter)
            {
                CountBasedQuest.Add(counter);
            }
            else if (questDto is QuestDto conditional)
            {
                ConditionalQuest.Add(conditional);
            }
        }
        public void Remove(IQuest questDto)
        {
            if (questDto is QuestCounterDto counter)
            {
                CountBasedQuest.Remove(counter);
            }
            else if (questDto is QuestDto conditional)
            {
                ConditionalQuest.Remove(conditional);
            }
        }
    }
}