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
}