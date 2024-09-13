using System;
using System.Collections.Generic;
using Meta.Configs;

namespace Meta.Controllers
{
    public interface IChangeController
    {
        void Process(ChangeConfig change);
    }
    
    public class ChangeController : IChangeController
    {
        private readonly IChangeControllersFactory _changeControllersFactory;
        private readonly Dictionary<TypeChange, IChangeController> _controllersHash;

        public ChangeController(IChangeControllersFactory changeControllersFactory)
        {
            _changeControllersFactory = changeControllersFactory;
            _controllersHash = new Dictionary<TypeChange, IChangeController>(16);

            //Создать на фабрике не могу, так как будет цикличная зависимость, пока думаю что с этим делать и нужно ли это.
            _controllersHash.Add(TypeChange.ChangesArray, new ChangeArrayController(this));
        }

        public void Process(ChangeConfig change)
        {
            if (_controllersHash.TryGetValue(change.TypeChange, out var processor) == false)
            {
                processor = _changeControllersFactory.Create(change);
                _controllersHash.Add(change.TypeChange, processor);
            }
            processor.Process(change);
        }
        
        private class ChangeArrayController : IChangeController
        {
            private readonly IChangeController _changeController;

            public ChangeArrayController(IChangeController changeController)
            {
                _changeController = changeController;
            }

            public void Process(ChangeConfig change)
            {
                var config = (ChangesConfig) change;
                foreach (var changeConfig in config.Changes)
                {
                    _changeController.Process(changeConfig);
                }
            }
        }
    }

    

    
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


    
    public class ChangeAddItemController : IChangeController
    {
        private readonly IInventoryController _inventoryController;

        public ChangeAddItemController(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void Process(ChangeConfig change)
        {
            var configChangeAddItem = (ConfigChangeAddItem) change;
            _inventoryController.Add(configChangeAddItem.TargetItem, configChangeAddItem.Count);
        }
    }
    public class ChangeSpendItemController : IChangeController
    {
        private readonly IInventoryController _inventoryController;

        public ChangeSpendItemController(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void Process(ChangeConfig change)
        {
            var configChangeSpendItem = (ConfigChangeSpendItem) change;
            _inventoryController.Spend(configChangeSpendItem.TargetItem, configChangeSpendItem.Count);
        }
    }
    public class ChangeAddUnitController : IChangeController
    {
        private readonly IUnitsController _unitsController;

        public ChangeAddUnitController(IUnitsController unitsController)
        {
            _unitsController = unitsController;
        }

        public void Process(ChangeConfig change)
        {
            var configChangeAddUnit = (ConfigChangeAddUnit) change;
            _unitsController.Add(configChangeAddUnit.UnitType, configChangeAddUnit.Progression, configChangeAddUnit.Count);
        }
    }
}