using System.Collections.Generic;


namespace Application.MetaSrc.Models
{
    /*
     //Копилот отжог ) У меня не так много вещей планируется ))
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

    public class MetaModel
    {
        public int ConfigVersion;
        public List<MetaUnitDto> Units = new List<MetaUnitDto>();
        
        public override int GetHashCode() => HashHelper.GetHashCode(ConfigVersion, Units);
    }


    // Есть квесты, которые могут добавить юнитов игроку.
    // Тут они должны знать, в какой элемнт добавлять юнита.
    // Так как юниты различаются прогрессией.
    public class MetaUnitDto 
    {
        public Id UnitType;
        public int Count;
        public UnitProgressionDto Progression;
        
        public override int GetHashCode() => HashHelper.GetHashCode(UnitType, Count, Progression);
    }

    
    public class UnitProgressionDto
    {
        public int MeleeAttackLevel;
        public int RangedAttackLevel;
        public int HealthLevel;

        /*public int MeleeEquipmentLevel { get; set; }
        public int RangedEquipmentLevel { get; set; }
        public int DefenseEquipmentLevel { get; set; }*/
        //?? perks??
        
        public override int GetHashCode() => HashHelper.GetHashCode(MeleeAttackLevel, RangedAttackLevel, HealthLevel);
    }
}