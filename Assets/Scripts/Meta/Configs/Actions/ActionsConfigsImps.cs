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


    /// <summary>
    /// Пример читаемого конфига удобного для сериализации.
    /// </summary>
    public class ActionCollectionConfig : IActionConfig, IEnumerable<IActionConfig>
    {
        public TypeAction TypeAction => TypeAction.Collection;

        //хранение коллекции в типизированном виде
        public List<UnitActionConfig> Untis = new List<UnitActionConfig>();
        public List<ItemActionConfig> Items = new List<ItemActionConfig>();

        //хранение в виде абстракций
        public List<IActionConfig> Actions = new List<IActionConfig>();

        //получение и использование в абстракции.
        public IEnumerator<IActionConfig> GetEnumerator()
        {
            foreach (var unitActionConfig in Untis)
            {
                yield return unitActionConfig;
            }

            foreach (var itemAction in Items)
            {
                yield return itemAction;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}