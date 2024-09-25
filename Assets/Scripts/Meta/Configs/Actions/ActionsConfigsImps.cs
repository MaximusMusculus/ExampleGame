using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Configs.Actions
{
    public class ItemActionConfig : IActionConfig
    {
        public TypeMetaAction TypeMetaAction => MetaAction;

        public TypeMetaAction MetaAction; //availableList?
        public Id TypeItem;
        public int Count;
    }

    //наследоваться от ItemActionConfig? с добавлением Progression
    public class UnitActionConfig : IActionConfig
    {
        public TypeMetaAction TypeMetaAction => MetaAction;

        public TypeMetaAction MetaAction;
        public Id TypeUnit;
        public UnitProgressionDto Progression;
        public int Count;
    }

    public class ActionCollectionConfig : IActionConfig
    {
        public TypeMetaAction TypeMetaAction => TypeMetaAction.Collection;

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