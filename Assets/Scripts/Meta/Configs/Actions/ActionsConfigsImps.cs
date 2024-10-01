using System;
using System.Collections.Generic;
using AppRen;
using Meta.Controllers;
using Meta.Models;

namespace Meta.Configs.Actions
{

    public interface IInventoryAction
    {
        public void Visit(IInventoryActionVisitor visitor);
    }

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
        void ItemAdd(Id itemId, int count);
        void ItemSpend(Id itemId, int count);
        void ItemExpandLimit(Id itemId, int count);
    }


    public class InventoryActionConfig : IActionConfig, IInventoryAction
    {
        public string ActionGroup => TypeActionGroup.Inventory;

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


    public interface IUnitActionVisitor
    {
        void UnitAdd(Id typeUnit, UnitProgressionDto progression, int count);
        void UnitSpend(Id typeUnit, UnitProgressionDto progression, int count);
    }

    public interface IUnitAction
    {
        void Visit(IUnitActionVisitor visitor);
    }

    //-- implement
    public class UnitActionConfig : IActionConfig, IUnitAction
    {
        public string ActionGroup => TypeActionGroup.Units;

        public void Visit(IActionProcessor processor)
        {
            throw new NotImplementedException();
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

    //-- implement
    public class ActionCollectionConfig : IActionConfig
    {
        public string ActionGroup => TypeActionGroup.Collection;

        //хранение набора коллекции в типизированном виде
        //для удобной читаемости и сериализации/десериализации
        public readonly List<UnitActionConfig> Untis = new List<UnitActionConfig>();
        public readonly List<InventoryActionConfig> Items = new List<InventoryActionConfig>();

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