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

        protected override bool Check(ConditionCollectionConfig conditionsConfig)
        {
            foreach (var condition in conditionsConfig)
            {
                if (_conditionProcessor.Check(condition))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class ConditionProcessorAndCollection : ConditionProcessorAbstract<ConditionCollectionConfig>
    {
        private readonly IConditionProcessor _conditionProcessor;

        public ConditionProcessorAndCollection(IConditionProcessor conditionProcessor)
        {
            _conditionProcessor = conditionProcessor;
        }

        protected override bool Check(ConditionCollectionConfig conditionsConfig)
        {
            foreach (var condition in conditionsConfig)
            {
                if (_conditionProcessor.Check(condition) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}