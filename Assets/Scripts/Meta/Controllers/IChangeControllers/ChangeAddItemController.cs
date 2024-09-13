using Meta.Configs;

namespace Meta.Controllers
{
    public class ChangeAddItemController : IChangeController
    {
        private readonly IInventoryController _inventoryController;

        public ChangeAddItemController(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void Process(ChangeConfig change)
        {
            var configChangeAddItem = (ChangeAddItemConfig) change;
            _inventoryController.Add(configChangeAddItem.TargetItem, configChangeAddItem.Count);
        }
    }
}