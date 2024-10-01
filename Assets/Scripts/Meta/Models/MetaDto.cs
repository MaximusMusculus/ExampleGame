using System.Collections.Generic;


namespace Meta.Models
{
    public class MetaDto
    {
        public int ConfigVersion;
        public List<ItemDto> Items = new List<ItemDto>();
        public List<UnitDto> Units = new List<UnitDto>();
        public QuestCollectionDto Quests = new QuestCollectionDto();

        //public List<PerkDto> Perkypecons;
        //public List<TaskDto> Tasks;
        //public List<BuildingDto> Buildings;
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