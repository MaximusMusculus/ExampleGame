using System;
using System.Collections.Generic;
using Meta.Configs;
using Meta.Models;

namespace Meta.Controllers
{
    public enum TypeUnitStat
    {
        Melee,
        Ranged,
        Health,
            
        MeleeWeapon,
        RangedWeapon,
        HealthArmor,
            
        Perks,
    }
    
    public interface IUnitStat
    {
        TypeUnitStat Stat { get;}
        int Level { get; }
    }


    /// <summary>
    /// Занимается расчетом статов юнита, получает на входе конфиг и данные прокачки.
    /// </summary>
    public class UnitStatsModel
    {
        private readonly UnitProgressionDto _progressionDto;
        private readonly List<UnitStat> _unitStats;

        private readonly UnitConfig _unitConfig; //stats calculator
        //private readonly IUnitStatsCalculator _unitStatsCalculator; 

        public int Level { get; private set; } //sum all upgrades
        public int MeleeAttack { get; private set;}     //calculated value
        public int RangedAttack { get; private set; }    //calculated value
        public int Health { get; private set;}          //calculated value
        

        public UnitStatsModel(UnitConfig config, UnitProgressionDto progressionDto)
        {
            _unitConfig = config;
            _progressionDto = progressionDto;
            _unitStats = new List<UnitStat>(3);//from config
            _unitStats.Add(new UnitStat(_progressionDto, TypeUnitStat.Melee));
            _unitStats.Add(new UnitStat(_progressionDto, TypeUnitStat.Ranged));
            _unitStats.Add(new UnitStat(_progressionDto, TypeUnitStat.Health));
            CalculateStats();
        }
        
        private void CalculateStats()
        {
            Level = _progressionDto.HealthLevel + _progressionDto.RangedAttackLevel + _progressionDto.MeleeAttackLevel;
            
            //TODO: calculate stats
            MeleeAttack = _progressionDto.MeleeAttackLevel * 10 + 5;
            RangedAttack = _progressionDto.RangedAttackLevel * 10 + 6;
            Health = _progressionDto.HealthLevel * 10 + 7;
        }

        //Должно ли это оставться открытым?
        public void UpgradeHandler()
        {
            CalculateStats();
        }

        public IEnumerable<IUnitStat> GetStats()
        {
            return _unitStats;
        }
        
        private class UnitStat : IUnitStat
        {
            private readonly UnitProgressionDto _progressionDto;
            public TypeUnitStat Stat { get; private set; }
            public int Level
            {
                get
                {
                    return Stat switch
                    {
                        TypeUnitStat.Melee => _progressionDto.MeleeAttackLevel,
                        TypeUnitStat.Ranged => _progressionDto.RangedAttackLevel,
                        TypeUnitStat.Health => _progressionDto.HealthLevel,
                        TypeUnitStat.MeleeWeapon => 0,
                        TypeUnitStat.RangedWeapon => 0,
                        TypeUnitStat.HealthArmor => 0,
                        TypeUnitStat.Perks => 0,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            } 
            
            public UnitStat(UnitProgressionDto progressionDto, TypeUnitStat stat)
            {
                _progressionDto = progressionDto;
                Stat = stat;
            }
        }
    }
}