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

        protected override bool Check(CountConditionConfig conditionsConfig)
        {
            var count = _inventoryController.GetCount(conditionsConfig.TypeItem);
            return count.CheckCompareIsTrue(conditionsConfig.CompareType, conditionsConfig.Value);
        }
    }
    public class ConditionProcessorInventoryLimit : ConditionProcessorAbstract<CountConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionProcessorInventoryLimit(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(CountConditionConfig conditionsConfig)
        {
            var limit = _inventoryController.GetLimit(conditionsConfig.TypeItem);
            return limit.CheckCompareIsTrue(conditionsConfig.CompareType, conditionsConfig.Value);
        }
    }
    
    public class ConditionProcessorUnitsCount : ConditionProcessorAbstract<CountConditionConfig>
    {
        private readonly IUnitsController _unitsController;

        public ConditionProcessorUnitsCount(IUnitsController unitsController)
        {
            _unitsController = unitsController;
        }

        protected override bool Check(CountConditionConfig conditionsConfig)
        {
            foreach (var unit in _unitsController.GetUnits())
            {
                if (unit.UnitType.Equals(conditionsConfig.TypeItem))
                {
                    return unit.Count.CheckCompareIsTrue(conditionsConfig.CompareType, conditionsConfig.Value);
                }
            }
            //если нет юнитов, значит нет лимита?)
            return true;
        }
    }
    
}