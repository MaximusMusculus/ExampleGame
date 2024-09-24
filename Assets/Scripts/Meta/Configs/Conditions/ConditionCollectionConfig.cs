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
        public readonly List<ConditionCollectionConfig> Collections = new List<ConditionCollectionConfig>();
        
        //схож с ActionCollectionConfig, там тестирую _enumerator
        private IConditionConfig[] _conditionsHash;
        private void CreateHash()
        {
            _conditionsHash = new IConditionConfig[CheckItems.Count + Collections.Count];
            var i = 0;
            foreach (var item in CheckItems)
            {
                _conditionsHash[i] = item;
                i++;
            }
            foreach (var item in Collections)
            {
                _conditionsHash[i] = item;
                i++;
            }
        }
        
        public IConditionConfig[] GetConditions()
        {
            if (_conditionsHash == null)
            {
                CreateHash();
            }
            return _conditionsHash;
        }
    }
}