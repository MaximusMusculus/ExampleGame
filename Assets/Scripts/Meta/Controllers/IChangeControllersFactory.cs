using System;
using Meta.Configs;

namespace Meta.Controllers
{
    public interface IChangeControllersFactory
    {
        IChangeController Create(ChangeConfig change);
    }
    
    public class ChangeControllersFactory : IChangeControllersFactory
    {
        private readonly IInventoryController _inventoryController;
        private readonly IUnitsController _unitsController;

        public ChangeControllersFactory(IInventoryController inventoryController, IUnitsController unitsController)
        {
            _inventoryController = inventoryController;
            _unitsController = unitsController;
        }

        public IChangeController Create(ChangeConfig change)
        {
            return change.TypeChange switch
            {
                TypeChange.AddItem => new ChangeAddItemController(_inventoryController),
                TypeChange.SpendItem => new ChangeSpendItemController(_inventoryController),
                TypeChange.AddUnit => new ChangeAddUnitController(_unitsController),
                TypeChange.ChangesArray => throw new ArgumentException($"Processor {change.TypeChange} should not be created in factory"),

                _ => throw new ArgumentException("Processor not found for change type: " + change.TypeChange)
            };
        }
    }
}