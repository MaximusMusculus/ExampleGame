namespace Meta.Models
{
    public class UnitProgressionDto
    {
        public int MeleeAttackLevel;
        public int RangedAttackLevel;
        public int HealthLevel;

        /*public int MeleeEquipmentLevel { get; set; }
        public int RangedEquipmentLevel { get; set; }
        public int DefenseEquipmentLevel { get; set; }*/
        //?? perks??
        public override int GetHashCode() => HashHelper.GetHashCode(MeleeAttackLevel, RangedAttackLevel, HealthLevel);
    }
}