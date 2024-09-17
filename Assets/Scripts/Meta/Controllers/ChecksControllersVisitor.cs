using System.Collections.Generic;
using System.Linq;
using Meta.Configs;

namespace Meta.Controllers
{
    public class CheckEntityController : ICheckVisitor
    {
        private IUnitsController _unitsController;
        public bool Visit(CheckUnitLimit unitLimit)
        {
            var targetUnit = _unitsController.GetUnits().FirstOrDefault(s => s.UnitType == unitLimit.TypeUnit);
            return targetUnit.Count < unitLimit.Count;
        }

        public bool Visit(IEnumerable<ICheckEntityConfig> collection)
        {
            var result = true;
            foreach (var entity in collection)
            {
                result &= entity.CheckVisit(this);
                if (result == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}