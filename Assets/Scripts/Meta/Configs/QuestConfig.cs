using System;
using System.Collections.Generic;
using AppRen;
using Meta.Configs.Conditions;


namespace Meta.Configs
{
    public interface IQuestCollectionConfig
    {
        IEnumerable<IQuestConfig> GetAll();
    }
    public class QuestCollectionConfig : IQuestCollectionConfig
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

        public void Add(IQuestConfig questConfig)
        {
            if (questConfig is QuestCountBasedConfig countBasedConfig)
            {
                CountBased.Add(countBasedConfig);
                return;
            }
            if (questConfig is QuestConditionalConfig conditionalQuest)
            {
                ConditionBased.Add(conditionalQuest);
                return;
            }
            
            throw new ArgumentException("Unknown quest type:" + questConfig.TypeQuestGroup);
        }
    } 
    
    public enum TypeQuestGroup
    {
        None,
        CountBased,
        Conditional
    }
    
    public interface IQuestConfig
    {
        public Id QuestId { get; }
        public TypeQuestGroup TypeQuestGroup { get; }
        public IEnumerable<TypeQuest> GetTriggers();
        
        IActionConfig Reward { get; set; }
    }
    
    
    
    public enum TypeQuest
    {
        None,
        Collection,

        InventoryItemAdd, //ItemActionConfig
        InventoryItemSpend, //ItemActionConfig
        InventoryItemExpandLimit, //ItemActionConfig

        UnitAdd, //UnitActionConfig
        UnitSpend, //UnitActionConfig
    }

    
    /// <summary>
    /// Квест для меты со своим счетчиком
    /// </summary>
    public class QuestCountBasedConfig : IQuestConfig
    {
        public TypeQuestGroup TypeQuestGroup => TypeQuestGroup.CountBased;
        public TypeQuest TriggerAction;
        
        
        public Id QuestId { get; set; }
        public Id TargetEntityId;
        public int TargetValue;

        public IEnumerable<TypeQuest> GetTriggers()
        {
            yield return TriggerAction;
        }

        public IActionConfig Reward { get; set; }
    }


    //Квест использующий счетчик игрока (инвентарь, юнитов)
    //нужен ли мне он?
    public class QuestConditionalConfig : IQuestConfig
    {
        public TypeQuestGroup TypeQuestGroup => TypeQuestGroup.Conditional;
        
        public Id QuestId { get; set;}
        public HashSet<TypeQuest> Triggers = new HashSet<TypeQuest>();
        public ConditionCollectionConfig Condition;

        public IEnumerable<TypeQuest> GetTriggers()
        {
            return Triggers;
        }

        public IActionConfig Reward { get; set; }
    }
    
    

}