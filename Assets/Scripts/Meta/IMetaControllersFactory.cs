using System.Collections.Generic;
using Meta.Configs;
using Meta.Controllers;
using Meta.Controllers.Actions;
using Meta.Controllers.Conditions;
using Meta.Controllers.Imp;
using Meta.Models;

namespace Meta
{
    public interface IMetaControllersFactory
    {
        IInventoryController CreateInventoryController(List<ItemDto> items);
        IUnitsController CreateUnitsController(List<UnitDto> units);
        
        //убрать передачу параметров, сделать регистрацию обработчиков снаружи.
        IActionProcessor CreateActionProcessor(IInventoryController inventoryController, IUnitsController unitController);
        IConditionProcessor CreateConditionProcessor(IInventoryController inventoryController, IUnitsController unitController);
    }

    public class MetaControllersFactory : IMetaControllersFactory
    {
        private readonly MetaConfig _metaConfig;
        
        public MetaControllersFactory(MetaConfig metaConfig)
        {
            _metaConfig = metaConfig;
        }

        public IInventoryController CreateInventoryController(List<ItemDto> items)
        {
            return new InventoryController(_metaConfig.InventoryItems, items);
        }

        public IUnitsController CreateUnitsController(List<UnitDto> units)
        {
            return new UnitsController(_metaConfig.Units, units);
        }
        public IActionProcessor CreateActionProcessor(IInventoryController inventoryController, IUnitsController unitController)
        {
            return new ActionProcessor(inventoryController, unitController);
        }
        public IConditionProcessor CreateConditionProcessor(IInventoryController inventoryController, IUnitsController unitController)
        {
            return new ConditionProcessor(inventoryController, unitController);
        }
        
        //IEventController CreateEventController(playerUnitsController); 
        // var eventInventory = CreateCreateInventoryController(eventItems);
        // return new EventController(eventInventory, playerUnitsController);
    }
}