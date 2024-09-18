using System.Collections.Generic;
using Meta.Configs;
using Meta.Configs.Conditions;

namespace Meta.Controllers.Conditions
{
    public class ConditionInventoryItemCount : ConditionAbstract<ItemConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionInventoryItemCount(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(ItemConditionConfig conditionConfig)
        {
            var count = _inventoryController.GetCount(conditionConfig.TypeItem);
            //+compare
            return count > conditionConfig.Value;
        }
    }

    public class ConditionInventoryLimit : ConditionAbstract<ItemConditionConfig>
    {
        private readonly IInventoryController _inventoryController;

        public ConditionInventoryLimit(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override bool Check(ItemConditionConfig conditionConfig)
        {
            var limit = _inventoryController.GetLimit(conditionConfig.TypeItem);
            return limit > conditionConfig.Value;
        }
    }

    //может быть центральным, где все регают в него нужные чекеры
    //может быть разбит по кусочкам и реализовать интерфейс IChecker, где внутри пробегается по мелким чекерам, спрашивая, могут ли они обработать аргсу
    public class CheckProcessor : ICondition
    {
        private readonly Dictionary<TypeCondition, ICondition> _checkers = new Dictionary<TypeCondition, ICondition>();

        public CheckProcessor(IInventoryController inventoryController)
        {
            _checkers.Add(TypeCondition.InventoryItemsCount, new ConditionInventoryItemCount(inventoryController));
            _checkers.Add(TypeCondition.InventoryItemsLimit, new ConditionInventoryLimit(inventoryController));
        }

        public void AddChecker(TypeCondition typeCondition, ICondition condition)
        {
            _checkers.Add(typeCondition, condition);
        }

        public bool Check(IConditionConfig conditionConfig)
        {
            if (_checkers.TryGetValue(conditionConfig.TypeCondition, out var checker))
            {
                return checker.Check(conditionConfig);
            }

            throw new System.ArgumentException($"Checker not found for {conditionConfig.TypeCondition}");
        }
    }


    public class CheckUse
    {
        private ICondition _checkProcessor;

        public void Test()
        {
            _checkProcessor.Check(new ItemConditionConfig {TypeCondition = TypeCondition.InventoryItemsCount, TypeItem = 1, Value = 50});
        }
    }

}