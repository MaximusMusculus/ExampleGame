using System.Collections.Generic;
using Meta.Configs;
using Meta.Configs.Conditions;

namespace Meta.Controllers.Conditions
{
    //может быть центральным, где все регают в него нужные чекеры
    //может быть разбит по кусочкам и реализовать интерфейс IChecker, где внутри пробегается по мелким чекерам, спрашивая, могут ли они обработать аргсу
    public class ConditionProcessor : IConditionProcessor
    {
        private readonly Dictionary<TypeCondition, IConditionProcessor> _checkers = new Dictionary<TypeCondition, IConditionProcessor>();

        public ConditionProcessor(IInventoryController inventoryController, IUnitsController unitsController)
        {
            _checkers.Add(TypeCondition.AndCollection, new ConditionProcessorAndCollection(this));
            _checkers.Add(TypeCondition.OrCollection, new ConditionProcessorOrCollection(this));

            _checkers.Add(TypeCondition.InventoryItemsCount, new ConditionProcessorInventoryItemCount(inventoryController));
            _checkers.Add(TypeCondition.InventoryItemsLimit, new ConditionProcessorInventoryLimit(inventoryController));
            
            _checkers.Add(TypeCondition.UnitsCount, new ConditionProcessorUnitsCount(unitsController));
        }

        public void AddChecker(TypeCondition typeCondition, IConditionProcessor conditionProcessor)
        {
            _checkers.Add(typeCondition, conditionProcessor);
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
        private IConditionProcessor _checkProcessor;

        public void Test()
        {
            _checkProcessor.Check(new CountConditionConfig {TypeCondition = TypeCondition.InventoryItemsCount, TypeItem = 1, Value = 50});
        }
    }
}