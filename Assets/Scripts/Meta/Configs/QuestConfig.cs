using System.Collections.Generic;
using AppRen;
using Meta.Configs.Conditions;


namespace Meta.Configs
{
    public class QuestCollectionConfig
    {
        public List<QuestCountBasedConfig> CountBased = new List<QuestCountBasedConfig>();
        public List<QuestConditionalConfig> ConditionBased = new List<QuestConditionalConfig>();
        
        public IEnumerable<IQuestConfig> GetAll()
        {
            foreach (var quest in CountBased)
            {
                yield return quest;
            }
            foreach (var quest in ConditionBased)
            {
                yield return quest;
            }
        }
    } 
    
    public enum TypeQuest
    {
        None,
        CountBased,
        Conditional
    }
    
    public interface IQuestConfig
    {
        public Id QuestId { get; }
        public TypeQuest TypeQuest { get; }
        IActionConfig Reward { get; set; }
    }
    
    
    public enum TypeCountBasedChange
    {
        None,
        InventoryItemAdd,           //ItemActionConfig
        InventoryItemSpend,         //ItemActionConfig
        InventoryItemExpandLimit,   //ItemActionConfig

        UnitAdd,        //UnitActionConfig
        UnitSpend,      //UnitActionConfig
    }
    
    
    
    /// <summary>
    /// Квест для меты со своим счетчиком
    /// </summary>
    public class QuestCountBasedConfig : IQuestConfig
    {
        public TypeQuest TypeQuest => TypeQuest.CountBased;
        public Id QuestId { get; set; }
        public TypeMetaAction TargetAction;
        public Id TargetEntityId;
        public int TargetValue;
        
        public IActionConfig Reward { get; set; }
    }


    //Квест использующий счетчик игрока (инвентарь, юнитов)
    //нужен ли мне он?
    public class QuestConditionalConfig : IQuestConfig
    {
        public TypeQuest TypeQuest => TypeQuest.Conditional;
        
        public Id QuestId { get; set;}
        public HashSet<TypeMetaAction> Triggers = new HashSet<TypeMetaAction>();
        public CountConditionConfig Condition;
        
        public IActionConfig Reward { get; set; }
    }
    
    

}