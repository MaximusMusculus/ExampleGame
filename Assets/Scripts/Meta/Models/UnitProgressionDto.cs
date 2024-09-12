using System.Collections.Generic;

namespace Meta.Models
{
    public class UnitProgressionDto
    {
        public int MeleeAttackLevel { get; set; }
        public int RangedAttackLevel{ get; set; }
        public int HealthLevel{ get; set; }
    }
    
    public class UnitProgressionEqualsComparer : IEqualityComparer<UnitProgressionDto>
    {
        public bool Equals(UnitProgressionDto x, UnitProgressionDto y)
        {
            return x.MeleeAttackLevel == y.MeleeAttackLevel && x.RangedAttackLevel == y.RangedAttackLevel && x.HealthLevel == y.HealthLevel;
        }

        public int GetHashCode(UnitProgressionDto obj)
        {
            return obj.MeleeAttackLevel.GetHashCode() ^ obj.RangedAttackLevel.GetHashCode() ^ obj.HealthLevel.GetHashCode();
        }
    }
}  