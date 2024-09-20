using System.Collections;
using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Configs.Actions
{
    public class ItemActionConfig : IActionConfig
    {
        public TypeAction TypeAction => Action;

        public TypeAction Action; //availableList?
        public Id TypeItem;
        public int Count;
    }

    public class UnitActionConfig : IActionConfig
    {
        public TypeAction TypeAction => Action;

        public TypeAction Action;
        public Id TypeUnit;
        public UnitProgressionDto Progression;
        public int Count;
    }

    public class ActionCollectionConfig : IActionConfig, IEnumerable<IActionConfig>
    {
        public TypeAction TypeAction => TypeAction.Collection;

        //хранение набора коллекции в типизированном виде
        //для удобной читаемости и сериализации/десериализации
        public readonly List<UnitActionConfig> Untis = new List<UnitActionConfig>();
        public readonly List<ItemActionConfig> Items = new List<ItemActionConfig>();

        private List<IActionConfig> _actionConfigs;
        private IEnumerator<IActionConfig> _enumerator;
        
        

        public IEnumerable<IActionConfig> GetActions()
        {
            if (_actionConfigs == null)
            {
                _actionConfigs = new List<IActionConfig>();
                foreach (var itemAction in Items)
                {
                    _actionConfigs.Add(itemAction);
                }

                foreach (var unitActionConfig in Untis)
                {
                    _actionConfigs.Add(unitActionConfig);
                }
            }
            _enumerator = _actionConfigs.GetEnumerator();
            return _actionConfigs;
        }

        //получение и использование в абстракции.
        public IEnumerator<IActionConfig> GetEnumerator()
        {
            if (_actionConfigs == null)
            {
                GetActions();
            }
            _enumerator.Reset();
            return _enumerator;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            _enumerator.Reset();
            return _enumerator;
        }
    }
}