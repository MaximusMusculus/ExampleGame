using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Controllers;
using Meta.Models;

namespace Meta.Configs
{
    public interface IVisitor
    {
        void Visit(EntityItem entityItem);
        void Visit(EntityUnit entityItem);
        void Visit(IEntityCollection collection);
    }

    
    /// <summary>
    /// Сущность, содержит логику взаимодействия. Конкретно эта - то, как списать предметы
    /// </summary>
    public class SpendVisitor : IVisitor
    {
        private IInventoryController _inventoryController;
        private IUnitsController _unitsController;
        
        public void Visit(EntityItem entityItem)
        {
            _inventoryController.Spend(entityItem.TypeItem, entityItem.Count);
        }
        
        public void Visit(EntityUnit entityItem)
        {
            //так же можно найти юнитов с учетом прогрессии.
            var unit =_unitsController.GetUnits().FirstOrDefault(s => s.UnitType == entityItem.TypeUnit);
            _unitsController.Spend(unit, entityItem.Count);
        }
        
        public void Visit(IEntityCollection collection)
        {
            foreach (var entity in collection.Get())
            {
                entity.Visit(this);
            }
        }
    }

    /// <summary>
    /// Сущность, содержит логику взаимодействия. Конкретно эта - как добавить предметы
    /// </summary>
    public class AddVisitor : IVisitor
    {
        private IInventoryController _inventoryController;
        private IUnitsController _unitsController;
        
        public void Visit(EntityItem entityItem)
        {
            _inventoryController.Add(entityItem.TypeItem, entityItem.Count);
        }
        public void Visit(EntityUnit entityItem)
        {
            _unitsController.Add(entityItem.TypeUnit, entityItem.Progression, entityItem.Count);
        }

        public void Visit(IEntityCollection collection)
        {
            foreach (var entity in collection.Get())
            {
                entity.Visit(this);
            }
        }
    }

    
    public interface IEntity
    {
        void Visit(IVisitor visitor);
    }
    
    public class EntityItem : IEntity
    {
        public Id TypeItem { get; }
        public int Count { get; }
        
        public void Visit(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    public class EntityUnit : IEntity
    {
        public Id TypeUnit { get; }
        public UnitProgressionDto Progression { get; }
        public int Count { get; }

        public void Visit(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    

    public interface IEntityCollection
    {
        IEnumerable<IEntity> Get();
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
        public List<string> RequirementsConfig;
        
        public Changes Spend;
        public Changes Add;
        
        //Так же предусмотреть то, что это может происходить через таск (время?)
        //taskConfig?
    }
    
    public class Changes : IEntityCollection
    {
        private List<IEntity> Values = new List<IEntity>();
        public IEnumerable<IEntity> Get()
        {
            return Values.AsEnumerable();
        }
    }
    
    
    
    
    
}