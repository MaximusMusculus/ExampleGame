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
            //Если потребуются события, то надо будет поставить прослойку в IInventoryController, которая будет помещать нужное событие в очередь.
            //Или же просто будет являтся частью фасада с методами подписки, на все, что нужно.
        }
    }
}