using System.Collections.Generic;



namespace Meta.Configs.Conditions
{
    public class ConditionCollectionConfig : IConditionConfig
    {
        public TypeCondition TypeCondition => TypeCollection;

        public TypeCondition TypeCollection; //And.Or./Not
        public readonly List<CountConditionConfig> CheckItems = new List<CountConditionConfig>();
        //checkUnits
        //checkTechs
        //checkCounters
        public readonly List<ConditionCollectionConfig> Collection = new List<ConditionCollectionConfig>();
        
        //схож с ActionCollectionConfig, там тестирую _enumerator
        private IConditionConfig[] _conditionsHash;
        private void CreateHash()
        {
            _conditionsHash = new IConditionConfig[CheckItems.Count + Collection.Count];
            var i = 0;
            foreach (var item in CheckItems)
            {
                _conditionsHash[i] = item;
                i++;
            }
            foreach (var item in Collection)
            {
                _conditionsHash[i] = item;
                i++;
            }
        }
        
        public IConditionConfig[] Elems()
        {
            if (_conditionsHash == null)
            {
                CreateHash();
            }
            return _conditionsHash;
        }
    }
}