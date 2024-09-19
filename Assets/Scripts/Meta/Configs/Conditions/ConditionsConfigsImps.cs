using System.Collections;
using System.Collections.Generic;
using AppRen;

namespace Meta.Configs.Conditions
{
    public class ItemConditionConfig : IConditionConfig
    {
        public TypeCondition TypeCondition { get; set; }
        public Id TypeItem;
        public int CompareType;
        public int Value;
    }
    
    public class ConditionCollectionConfig : IConditionConfig, IEnumerable<IConditionConfig>
    {
        public TypeCondition TypeCondition => TypeCollection;
        public TypeCondition TypeCollection; //And.Or.Not?
        public List<IConditionConfig> Conditions = new List<IConditionConfig>();
        
        public IEnumerator<IConditionConfig> GetEnumerator()
        {
            return Conditions.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}