using AppRen;

namespace Meta.Configs.Conditions
{
    public class CountConditionConfig : IConditionConfig
    {
        public TypeCondition TypeCondition => Condition;

        public TypeCondition Condition;
        public Id TypeItem;
        public TypeCompare CompareType;
        public int Value;
    }
}