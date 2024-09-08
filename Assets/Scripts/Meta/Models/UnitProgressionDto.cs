using System;
using UnityEngine;

namespace Meta.Models
{
    [Serializable]
    public class UnitProgressionDto
    {
        public int MeleeAttackLevel;
        public int RangedAttackLevel;
        public int HealthLevel;

        
        // это может дописывать ИИ или генератор
        public override int GetHashCode() => HashHelper.GetHashCode(MeleeAttackLevel, RangedAttackLevel, HealthLevel);
    }
}