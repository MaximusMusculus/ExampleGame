using Meta.Configs.Conditions;

namespace Meta.Controllers.Conditions
{
    public class ConditionProcessorInventoryItemCount : ConditionProcessorAbstract<CountConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionProcessorInventoryItemCount(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(CountConditionConfig conditionConfig)
        {
            var count = _inventoryController.GetCount(conditionConfig.TypeItem);
            return count.CheckCompareIsTrue(conditionConfig.CompareType, conditionConfig.Value);
        }
    }
    public class ConditionProcessorInventoryLimit : ConditionProcessorAbstract<CountConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionProcessorInventoryLimit(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(CountConditionConfig conditionConfig)
        {
            var limit = _inventoryController.GetLimit(conditionConfig.TypeItem);
            return limit.CheckCompareIsTrue(conditionConfig.CompareType, conditionConfig.Value);
        }
    }
    
    public class ConditionProcessorUnitsCount : ConditionProcessorAbstract<CountConditionConfig>
    {
        private readonly IUnitsController _unitsController;

        public ConditionProcessorUnitsCount(IUnitsController unitsController)
        {
            _unitsController = unitsController;
        }

        protected override bool Check(CountConditionConfig args)
        {
            foreach (var unit in _unitsController.GetUnits())
            {
                if (unit.UnitType.Equals(args.TypeItem))
                {
                    return unit.Count.CheckCompareIsTrue(args.CompareType, args.Value);
                }
            }
            //если нет юнитов, значит нет лимита?)
            return true;
        }
    }
    
}