using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta.Controllers
{
    public class UnitModel : IUnitModel
    {
        public UnitConfig Config { get; } //??
        public Id UnitType { get; }
        public UnitStatsModel Stats { get; }

        private readonly UnitDto _dtoCount;

        public int Count => _dtoCount.Count;

        public UnitModel(UnitConfig config, UnitStatsModel stats, UnitDto dtoCount)
        {
            Config = config;
            UnitType = config.UnitType;
            Stats = stats;
            _dtoCount = dtoCount;
        }
    }

    public class UnitsController : IUnitsController, IUnitsProgression
    {
        private readonly Dictionary<Id, UnitConfig> _configsHash;
        private readonly List<UnitDto> _unitsDto;
        private readonly Dictionary<IUnitModel, UnitDto> _unitModels;
        private readonly IEqualityComparer<UnitProgressionDto> _progressionComparer = new UnitProgressionEqualsComparer();

        public UnitsController(List<UnitConfig> unitConfigs, List<UnitDto> unitsDto)
        {
            _unitsDto = unitsDto;
            _configsHash = new Dictionary<Id, UnitConfig>(unitConfigs.Count);
            _unitModels = new Dictionary<IUnitModel, UnitDto>(_unitsDto.Count);

            CreateConfigsHash(unitConfigs);
            CreateUnitModels(unitsDto);
        }

        private void CreateConfigsHash(List<UnitConfig> unitConfigs)
        {
            foreach (var unitConfig in unitConfigs)
            {
                _configsHash[unitConfig.UnitType] = unitConfig;
            }
        }
        private void CreateUnitModels(List<UnitDto> units)
        {
            foreach (var unitDto in units)
            {
                var config = _configsHash[unitDto.UnitType];
                _unitModels.Add(CreateUnitModel(config, unitDto), unitDto);
            }
        }
        private IUnitModel CreateUnitModel(UnitConfig config, UnitDto unitDto)
        {
            //Если создание модели станет сложным и захочет много зависимостей, то перенести в фабрику
            var unit = new UnitModel(config, new UnitStatsModel(config, unitDto.Progression), unitDto);
            return unit;
        }

        public void Add(Id unitType, int count)
        {
            var config = _configsHash[unitType];
            Add(unitType, config.GetDefaultProgression(), count);
        }
        public void Add(Id unitType, UnitProgressionDto progression, int count)
        {
            var config = _configsHash[unitType];
            if (config.IsCanUpgrade)
            {
                AddUpgradableUnit(config, progression, count);
            }
            else
            {
                AddNotUpgradableUnit(config, progression, count);
            }
        }

        private void AddUpgradableUnit(UnitConfig config, UnitProgressionDto progression, int count)
        {
            var unitDto = _unitsDto.FirstOrDefault(unitDto => unitDto.UnitType.Equals(config.UnitType));
            AddUnit(config, unitDto, progression, count);
        }
        private void AddNotUpgradableUnit(UnitConfig config, UnitProgressionDto progression, int count)
        {
            var unitDto = _unitsDto.FirstOrDefault(s => s.UnitType.Equals(config.UnitType) && _progressionComparer.Equals(s.Progression, progression));
            AddUnit(config, unitDto, progression, count);
        }

        private void AddUnit(UnitConfig config, UnitDto unitDto, UnitProgressionDto progression, int count)
        {
            if (unitDto != null)
            {
                IncreaseUnitCount(unitDto, count);
            }
            else
            {
                AddNewUnit(config, progression, count);
            }
        }
        private void IncreaseUnitCount(UnitDto unit, int count)
        {
            unit.Count = checked(unit.Count + count);
        }
        private void AddNewUnit(UnitConfig config, UnitProgressionDto progression, int count)
        {
            var unit = new UnitDto
            {
                UnitType = config.UnitType,
                Progression = progression,
                Count = count
            };
            _unitsDto.Add(unit);
            _unitModels.Add(CreateUnitModel(config, unit), unit);
        }


        public void Spend(IUnitModel unitModel, int count)
        {
            Assert.IsTrue(unitModel.Count >= count);

            var dto = _unitModels[unitModel];
            var config = _configsHash[unitModel.UnitType];
            dto.Count -= count;

            if (dto.Count > 0 || config.IsCanUpgrade)
            {
                return;
            }

            _unitsDto.Remove(dto);
            _unitModels.Remove(unitModel);
        }
        public IEnumerable<IUnitModel> GetUnits()
        {
            return _unitModels.Keys.Where(s => s.Count > 0);
        }


        //--спорное решение держать этот здесь.
        public void Upgrade(IUnitModel unit, TypeUnitStat stat)
        {
            var config = _configsHash[unit.UnitType];
            Assert.IsTrue(config.IsCanUpgrade);
            var unitDto = _unitModels[unit];
            var unitProgressionDto = unitDto.Progression;

            //todo сделать полноценную грейдилку (проверка на лимиты, работает с конфигом и дто) возможно по Type из конфига
            switch (stat)
            {
                case TypeUnitStat.Melee:
                    unitProgressionDto.MeleeAttackLevel++;
                    break;
                case TypeUnitStat.Ranged:
                    unitProgressionDto.RangedAttackLevel++;
                    break;
                case TypeUnitStat.Health:
                    unitProgressionDto.HealthLevel++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }

            unit.Stats.UpgradeHandler();
        }

        public IEnumerable<IUnitModel> GetCanUpgradeUnits()
        {
            foreach (var model in _unitModels)
            {
                if (_configsHash[model.Key.UnitType].IsCanUpgrade)
                {
                    yield return model.Key;
                }
            }
        }
    }
    
}