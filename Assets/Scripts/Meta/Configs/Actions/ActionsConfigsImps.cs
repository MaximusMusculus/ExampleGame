using System;
using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Configs.Actions
{
    /// Этот интерфейс будет юзаться в
    /// 1. ExecuteProcessor
    /// 2. QuestProcessor
    /// 3. CounterProcessor
    /// 4. ViewProcessor?
    ///
    /// Из минусов - если что то одно хочу потрекать (например в квесте, то надо будет хендлить ВСЕ методы)
    /// Из плюсов - не потеряется, если что то добавлю. Но нужно ли это все?
    /// Можно попробовать оба варианта 
    public interface IInventoryActionVisitor
    {
        void ItemAdd(ItemActionConfig itemActionConfig);
        void ItemSpend(ItemActionConfig itemActionConfig);
        void ItemExpandLimit(ItemActionConfig itemActionConfig);
    }
    
    public interface IInventoryAction
    {
        public void Visit(IInventoryActionVisitor visitor);
    }
    
    //-- implement
    
    public class ItemActionConfig : IActionConfig, IInventoryAction
    {
        public string ActionGroup => TypeActionGroup.Inventory;
        
        public TypeInventoryAction Action;
        public Id TypeItem;
        public int Count;

        public ItemActionConfig(TypeInventoryAction action)
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
                    visitor.ItemAdd(this);
                    break;
                case TypeInventoryAction.ItemSpend:
                    visitor.ItemSpend(this);
                    break;
                case TypeInventoryAction.ItemExpandLimit:
                    visitor.ItemExpandLimit(this);
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
    
    
    //----------Units------------------
    public interface IUnitActionVisitor
    {
        void UnitAdd(UnitActionConfig unitActionConfig);
        void UnitSpend(UnitActionConfig unitActionConfig);
    }
    public interface IUnitAction
    {
        void Visit(IUnitActionVisitor visitor);
    }
    
    //-- implement
   public class UnitActionConfig : IActionConfig, IUnitAction
    {
        public string ActionGroup => TypeActionGroup.Units;
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
                    visitor.UnitAdd(this);
                    break;
                case TypeUnitAction.UnitSpend:
                    visitor.UnitSpend(this);
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
    
    //-- implement
    public class ActionCollectionConfig : IActionConfig
    {
        public TypeMetaAction TypeMetaAction => TypeMetaAction.Collection;
        public string ActionGroup => TypeActionGroup.Collection;

        //хранение набора коллекции в типизированном виде
        //для удобной читаемости и сериализации/десериализации
        public readonly List<UnitActionConfig> Untis = new List<UnitActionConfig>();
        public readonly List<ItemActionConfig> Items = new List<ItemActionConfig>();
        
        //схож с ConditionCollectionConfig, там тестирую массив
        private IActionConfig[] _actionsHash;
        
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
                _actionsHash[i]= unit;
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