using System.Linq;
using Meta.Configs.Conditions;

namespace Meta.Controllers.Conditions
{
    public class ConditionProcessorOrCollection : ConditionProcessorAbstract<ConditionCollectionConfig>
    {
        private readonly IConditionProcessor _conditionProcessor;

        public ConditionProcessorOrCollection(IConditionProcessor conditionProcessor)
        {
            _conditionProcessor = conditionProcessor;
        }

        protected override bool Check(ConditionCollectionConfig config)
        {
            return config.Any(orCondition => _conditionProcessor.Check(orCondition));
        }
    }
    
    public class ConditionProcessorAndCollection : ConditionProcessorAbstract<ConditionCollectionConfig>
    {
        private readonly IConditionProcessor _conditionProcessor;

        public ConditionProcessorAndCollection(IConditionProcessor conditionProcessor)
        {
            _conditionProcessor = conditionProcessor;
        }

        protected override bool Check(ConditionCollectionConfig config)
        {
            return config.All(andCondition => _conditionProcessor.Check(andCondition));
        }
    }

}