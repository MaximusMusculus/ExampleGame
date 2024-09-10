using AppRen;

namespace Meta.Models
{
    public class UnitDto
    {
        public Id UnitType;
        public int Count;
        public UnitProgressionDto Progression;

        public override int GetHashCode() => HashHelper.GetHashCode(UnitType, Count, Progression);
    }
}