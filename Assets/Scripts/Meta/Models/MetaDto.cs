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
        
        
        //public List<PerkDto> Perkypecons;
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
        public Id Id{ get; set; } //могут ли быть 2 один-х квеста?
        //наверное - нет.

        public QuestDto()
        {
        }

        public QuestDto(Id configId)
        {
            ConfigId = configId;
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
        public void Add(IQuest questDto)
        {
            if (questDto is QuestCounterDto counter)
            {
                CountBasedQuest.Add(counter);
            }
            else if(questDto is QuestDto conditional)
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