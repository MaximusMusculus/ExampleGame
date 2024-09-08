using System.Collections.Generic;
using Application;
using Meta.Configs;
using Meta.Models;


namespace Meta.ConfigOdin
{
    /// <summary>
    /// Как бы не было это бесячим, но нужно создавать парралельные классы конфигов для одина, если я хочу использовать атрибуты и прочее.
    /// Получится редактор конфига игры на одине.
    /// Выглядит дико, но один позволяет доп атрибутами хорошо кастомизировать редакторы.
    /// </summary>
    public class MetaConfigOdin
    {
        public List<ResourceConfigOdin> Resources;
        public List<UnitConfigOdin> Units;
    }

    public class ResourceConfigOdin
    {
        public Id ResourceType;
        public string TechName;
        //поля локализации и описания можно не добавлять сюда, а добавлять в соответствующие массивы, а тут отображать как связанные данные. 
    }
    
    public class UnitConfigOdin
    {
        public Id UnitType;
        public string TechName;
        public TypeUnitCalculation TypeCalculation;
    }
    
    
    
    public class QuestConfigOdin //
    {
        public Id QuestType;    //уникальный id
        public string TechName; //поле, которое будет отображать техническое имя для удобства настройки
        public IReward Reward;  //выпадашка с типом награды
    }
    
    
    

    public interface IReward
    {
    }
    public class ListRewards : IReward
    {
        public List<IReward> Rewards;
    }
    public class AddResource : IReward
    {
        //[ResourceSelectedAttribute
        public Id ResourceType;
        public int Count; 
    }
    public class AddUnit : IReward
    {
        //[SelectedAttribute(typeof(UnitConfigOdin))] <-- ссылка на архитип юнита
        public Id UnitType;
        public UnitProgressionDto Progression;
        public int Count;
    }



}