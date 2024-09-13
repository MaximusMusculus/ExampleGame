using Meta.Configs;

namespace Meta.Controllers
{
    public class ChangeAddUnitController : IChangeController
    {
        private readonly IUnitsController _unitsController;

        public ChangeAddUnitController(IUnitsController unitsController)
        {
            _unitsController = unitsController;
        }

        public void Process(ChangeConfig change)
        {
            var configChangeAddUnit = (ChangeAddUnitConfig) change;
            _unitsController.Add(configChangeAddUnit.UnitType, configChangeAddUnit.Progression, configChangeAddUnit.Count);
        }
    }
}