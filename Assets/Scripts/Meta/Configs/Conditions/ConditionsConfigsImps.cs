using AppRen;

namespace Meta.Configs.Conditions
{
    public class CountConditionConfig : IConditionConfig
    {
        public TypeCondition TypeCondition { get; set; }
        
        public Id TypeItem;
        public TypeCompare CompareType;
        public int Value;
    }
}