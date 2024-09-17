using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Configs
{
    /// <summary>
    /// Так как логика выносится из отдельных контроллеров в один. То этот итерфейс будет разрастаться.
    /// важно соблюсти баланс между его размером и функционалом. Большое количество сущностей не планируется. 3-6 штук.
    /// </summary>
    public interface IChangeVisitor
    {
        void Visit(ChangeItemConfig changeItemConfig);
        void Visit(ChangeUnitConfig changeItem);
        void Visit(IEnumerable<IChangeEntityConfig> collection);
    }
    
    public interface IChangeEntityConfig
    {
        void Visit(IChangeVisitor changeVisitor);
    }
    
    //- imp
    public class ChangeItemConfig : IChangeEntityConfig
    {
        public Id TypeItem { get; }
        public int Count { get; }

        public ChangeItemConfig(Id typeItem, int count)
        {
            TypeItem = typeItem;
            Count = count;
        }

        public void Visit(IChangeVisitor changeVisitor)
        {
            changeVisitor.Visit(this);
        }
    }
    public class ChangeUnitConfig : IChangeEntityConfig
    {
        public Id TypeUnit { get; }
        public UnitProgressionDto Progression { get; }
        public int Count { get; }

        public ChangeUnitConfig(Id typeUnit, UnitProgressionDto progression, int count)
        {
            TypeUnit = typeUnit;
            Progression = progression;
            Count = count;
        }

        public void Visit(IChangeVisitor changeVisitor)
        {
            changeVisitor.Visit(this);
        }
    }
    
   

        
    /// <summary>
    /// Тут, логика будет содержаться в SpendVisitor, AddVisitor
    /// конфиг содержит только данные. А логика добавления/изымания отдельно.
    /// В зависимости от условий она подставляеся.
    /// Так же может появится ViewVisitor который будет посещать конфиг и говорить о том, что будет добавлятся или удаляться
    /// </summary>
    public class ExchangeConfig
    {
        /// <summary>
        /// ограничения.
        /// Может быть макс количество предметов
        /// Может быть открытие технологии. Или что то по сюжету.
        /// ItemLimit = 50 - заданный в конфигах лимит
        /// Need (Item, Level, Tech, ...)
        /// </summary>
        public List<ICheckEntityConfig> Requirements;
        public List<IChangeEntityConfig> Spend;
        public List<IChangeEntityConfig> Add;
        
        //Так же предусмотреть то, что это может происходить через таск (время?)
        //taskConfig?
    }
}
