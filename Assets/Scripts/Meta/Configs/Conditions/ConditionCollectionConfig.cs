using System.Collections.Generic;

namespace Meta.Configs.Conditions
{
    public class ConditionCollectionConfig : IConditionConfig
    {
        public TypeCondition TypeCondition => TypeCollection;

        public TypeCondition TypeCollection; //And.Or./Not
        public List<ConditionCollectionConfig> Collection = new List<ConditionCollectionConfig>();

        public List<CountConditionConfig> CheckItems = new List<CountConditionConfig>();
        //checkUnits
        //checkTechs
        //checkCounters

        public IEnumerator<IConditionConfig> GetEnumerator()
        {
            foreach (var item in CheckItems)
            {
                yield return item;
            }

            foreach (var collection in Collection)
            {
                yield return collection;
            }
        }
    }
}