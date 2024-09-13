using AppRen;
using Meta.Models;

namespace Meta.Configs
{
    public class ChangeAddUnitConfig : ChangeConfig
    {
        public override TypeChange TypeChange => TypeChange.AddUnit;
        public Id UnitType;
        public UnitProgressionDto Progression;
        public int Count;
    }
}