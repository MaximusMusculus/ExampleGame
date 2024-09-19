using System.Linq;
using Meta.Configs.Conditions;

namespace Meta.Controllers.Conditions
{
    public class ConditionProcessorInventoryItemCount : ConditionProcessorAbstract<ItemConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionProcessorInventoryItemCount(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(ItemConditionConfig conditionConfig)
        {
            var count = _inventoryController.GetCount(conditionConfig.TypeItem);
            return count.CheckCompareIsTrue(conditionConfig.CompareType, conditionConfig.Value);
        }
    }
    public class ConditionProcessorInventoryLimit : ConditionProcessorAbstract<ItemConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionProcessorInventoryLimit(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(ItemConditionConfig conditionConfig)
        {
            var limit = _inventoryController.GetLimit(conditionConfig.TypeItem);
            return limit.CheckCompareIsTrue(conditionConfig.CompareType, conditionConfig.Value);
        }
    }
    
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