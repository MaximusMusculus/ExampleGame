using System.Collections.Generic;
using AppRen;


namespace Meta.Models
{
    public class MetaDto
    {
        public int ConfigVersion;
        public List<ItemDto> Items = new List<ItemDto>(); //itemCollection
        public List<UnitDto> Units = new List<UnitDto>(); //unitCollection
        
        public List<ExchangeItemDto> PurchaseItems = new List<ExchangeItemDto>();
        public QuestCollectionDto Quests = new QuestCollectionDto();
        public QuestsDto PlayerQuests = new QuestsDto();

        //public List<PerkDto> Perks;
        //public List<TaskDto> Tasks;
        //public List<BuildingDto> Buildings;
    }

    public interface IQuest
    {
        public Id ConfigId { get; }
        public Id Id{ get; }
        
        public bool IsCompleted{ get; }
        public bool IsRewarded{ get; }
    }

    public class QuestDto : IQuest
    {
        public Id ConfigId { get; set; }
        public Id Id{ get; set; }

        public QuestDto()
        {
        }

        public QuestDto(Id configId, Id id)
        {
            ConfigId = configId;
            Id = id;
        }

        public bool IsCompleted{ get; set; }
        public bool IsRewarded{ get; set; }
    }
    

    public class QuestCounterDto : QuestDto
    {
        public int Value;

        public QuestCounterDto()
        {
        }

        public QuestCounterDto(Id configId, Id id) : base(configId, id)
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

        //Спрячу реализацию внутри, это сделано для оптимизации
        //и никто не будет знать ^_^
        public void Add(QuestDto questDto)
        {
            if (questDto is QuestCounterDto counter)
            {
                CountBasedQuest.Add(counter);
            }
            else
            {
                ConditionalQuest.Add(questDto);
            }
        }
    }

    public interface IQuests
    {
        public IEnumerable<IQuest> GetAll();
        public bool TryGet(Id id, out IQuest quest);
        public bool TryGetCount (Id id, out int count);
    }

    //Могу завернуть в нотификации, тогда каждое действие - будет нотифицировать ^_^ 
    public interface IQuestsController 
    {
        void AddNewQuest(Id configId);
        void ClaimReward(Id id);
    }
    

    public class QuestsDto
    {
        public readonly List<QuestDto> Quests = new List<QuestDto>(ConstDefaultCapacity.Medium);
        public readonly Dictionary<Id, int> Counters = new Dictionary<Id, int>(ConstDefaultCapacity.Small);

        public void Add(QuestDto questDto)
        {
            Quests.Add(questDto);
        }
        
        public void Remove(QuestDto questId)
        {
            Quests.Remove(questId);
            Counters.Remove(questId.Id);
        }
    }

    /*
 //Копилот торопит события. ^_^
 public class PlayerDto
{
    public Id Id;
    public string Name;
    public int Level;
    public int Experience;
    public int Gold;
    public int Gems;
    public List<PlayerUnitDto> Units;
    public List<PlayerItemDto> Items;
    public List<PlayerBuildingDto> Buildings;
    public List<PlayerResourceDto> Resources;
    public List<PlayerQuestDto> Quests;
    public List<PlayerAchievementDto> Achievements;
    public List<PlayerStoryDto> Stories;
    public List<PlayerCharacterDto> Characters;
    public List<PlayerArtifactDto> Artifacts;
    public List<PlayerSpellDto> Spells;
    public List<PlayerSkillDto> Skills;
    public List<PlayerPerkDto> Perks;
    public List<PlayerBuffDto> Buffs;
    public List<PlayerDebuffDto> Debuffs;
    public List<PlayerEffectDto> Effects;
    public List<PlayerEventDto> Events;
    public List<PlayerLocationDto> Locations;
    public List<PlayerTaskDto> Tasks;
    public List<PlayerShopDto> Shops;
    public List<PlayerShopItemDto> ShopItems;
    public List<PlayerShopTabDto> ShopTabs;
    public List<PlayerShopItemStyleDto> ShopItemStyles;
    public List<PlayerPaymentItemDto> Payment
}*/
    
}