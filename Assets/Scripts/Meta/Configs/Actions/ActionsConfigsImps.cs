using System.Collections.Generic;
using AppRen;
using Meta.Models;
using UnityEngine.Assertions;

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

    public class ActionCollectionConfig : IActionConfig
    {
        public TypeAction TypeAction => TypeAction.Collection;

        //хранение набора коллекции в типизированном виде
        //для удобной читаемости и сериализации/десериализации
        public readonly List<UnitActionConfig> Untis = new List<UnitActionConfig>();
        public readonly List<ItemActionConfig> Items = new List<ItemActionConfig>();
        
        //схож с ConditionCollectionConfig, там тестирую массив
        
        private IEnumerator<IActionConfig> _enumerator;
        private IEnumerator<IActionConfig> CreateHash()
        {
            var actionConfigs = new List<IActionConfig>(Items.Count + Untis.Count);
            actionConfigs.AddRange(Items);
            actionConfigs.AddRange(Untis);
            _enumerator = actionConfigs.GetEnumerator();
            return _enumerator;
        }

        //получение и использование в абстракции.
        public IEnumerator<IActionConfig> GetEnumerator()
        {
            if (_enumerator == null)
            {
                _enumerator = CreateHash();
            }
            else
            {
                Assert.IsFalse(_enumerator.MoveNext(), "Enumerator is not reset");
            }

            _enumerator.Reset();
            return _enumerator;
        }
    }
}