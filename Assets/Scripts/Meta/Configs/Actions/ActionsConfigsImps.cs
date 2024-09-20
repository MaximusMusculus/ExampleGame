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
        
        //получение и использование в абстракции.
        public IEnumerator<IActionConfig> GetEnumerator()
        {
            foreach (var itemAction in Items)
            {
                yield return itemAction;
            }
            foreach (var unitActionConfig in Untis)
            {
                yield return unitActionConfig;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    
}