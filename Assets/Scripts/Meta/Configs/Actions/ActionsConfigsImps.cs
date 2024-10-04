using System;
using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Configs.Actions
{
    public interface IInventoryAction
    {
        public void Visit(IInventoryActionVisitor visitor);
    }
    public interface IInventoryActionVisitor
    {
        void ItemAdd(Id itemId, int count);
        void ItemSpend(Id itemId, int count);
        void ItemExpandLimit(Id itemId, int count);
    }
    
    public interface IUnitAction
    {
        void Visit(IUnitActionVisitor visitor);
    }
    public interface IUnitActionVisitor
    {
        void UnitAdd(Id typeUnit, UnitProgressionDto progression, int count);
        void UnitSpend(Id typeUnit, UnitProgressionDto progression, int count);
    }

    public interface IActionCollectionConfig : IActionConfig
    {
        IActionConfig[] GetAll();
    }
}


//-- implement
namespace Meta.Configs.Actions.Imp
{
    public class InventoryActionConfig : IActionConfig, IInventoryAction
    {
        public string ActionGroup => TypeActionGroup.Inventory;
        public void Accept(IActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TypeInventoryAction Action;
        public Id TypeItem;
        public int Count;

        public InventoryActionConfig(TypeInventoryAction action)
        {
            Action = action;
        }

        public override string ToString()
        {
            return $"TypeItem: {TypeItem}, Count: {Count}";
        }

        public void Visit(IInventoryActionVisitor visitor)
        {
            switch (Action)
            {
                case TypeInventoryAction.ItemAdd:
                    visitor.ItemAdd(TypeItem, Count);
                    break;
                case TypeInventoryAction.ItemSpend:
                    visitor.ItemSpend(TypeItem, Count);
                    break;
                case TypeInventoryAction.ItemExpandLimit:
                    visitor.ItemExpandLimit(TypeItem, Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum TypeInventoryAction
        {
            None,
            ItemAdd,
            ItemSpend,
            ItemExpandLimit
        }
    }
    
    public class UnitActionConfig : IActionConfig, IUnitAction
    {
        public string ActionGroup => TypeActionGroup.Units;
        public void Accept(IActionVisitor visitor)
        {
            visitor.Visit(this);
        }
        

        public TypeUnitAction TypeAction;

        public Id TypeUnit;
        public UnitProgressionDto Progression;
        public int Count;

        public UnitActionConfig(TypeUnitAction typeAction)
        {
            TypeAction = typeAction;
        }

        public override string ToString()
        {
            return $"TypeUnit: {TypeUnit}, Progression: {Progression}, Count: {Count}";
        }

        public void Visit(IUnitActionVisitor visitor)
        {
            switch (TypeAction)
            {
                case TypeUnitAction.UnitAdd:
                    visitor.UnitAdd(TypeUnit, Progression, Count);
                    break;
                case TypeUnitAction.UnitSpend:
                    visitor.UnitSpend(TypeUnit, Progression, Count);
                    break;
                default:
                    throw new ArgumentException("Unknown action " + TypeAction);
            }
        }

        public enum TypeUnitAction
        {
            None,
            UnitAdd,
            UnitSpend,
        }
    }
    
    public class ActionCollectionConfig : IActionCollectionConfig
    {
        public string ActionGroup => TypeActionGroup.Collection;

        public void Accept(IActionVisitor visitor)
        {
            foreach (var actionConfig in GetAll())
            {
                actionConfig.Accept(visitor);
            }
        }
        
    
        public IEnumerable<IActionConfig> GetForGroup(string groupName)
        {
            var collection = GetAll();
            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i].ActionGroup == groupName)
                {
                    yield return collection[i];
                }
            }
        }

        //хранение набора коллекции в типизированном виде
        //для удобной читаемости и сериализации/десериализации
        public List<UnitActionConfig> Untis = new List<UnitActionConfig>();
        public List<InventoryActionConfig> Items = new List<InventoryActionConfig>();

        //схож с ConditionCollectionConfig, там тестирую массив
        private IActionConfig[] _actionsHash;

        public void Add(IActionConfig actionConfig)
        {
            switch (actionConfig)
            {
                case InventoryActionConfig inventoryActionConfig:
                    Items.Add(inventoryActionConfig);
                    break;
                case UnitActionConfig unitActionConfig:
                    Untis.Add(unitActionConfig);
                    break;
                default:
                    throw new ArgumentException("Unknown action type " + actionConfig);
            }
        }

        private void CreateHash()
        {
            _actionsHash = new IActionConfig[Items.Count + Untis.Count];
            int i = 0;

            foreach (var item in Items)
            {
                _actionsHash[i] = item;
                i++;
            }

            foreach (var unit in Untis)
            {
                _actionsHash[i] = unit;
                i++;
            }
        }

        //получение и использование в абстракции.
        public IActionConfig[] GetAll()
        {
            if (_actionsHash == null)
            {
                CreateHash();
            }

            return _actionsHash;
        }
        
    }
    
}