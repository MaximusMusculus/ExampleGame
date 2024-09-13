using Meta.Configs;

namespace Meta.Controllers
{
    public class ChangeSpendItemController : IChangeController
    {
        private readonly IInventoryController _inventoryController;

        public ChangeSpendItemController(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void Process(ChangeConfig change)
        {
            var configChangeSpendItem = (ChangeSpendItemConfig) change;
            _inventoryController.Spend(configChangeSpendItem.TargetItem, configChangeSpendItem.Count);
        }
    }
}