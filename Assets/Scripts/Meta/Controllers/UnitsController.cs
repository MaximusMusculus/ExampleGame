using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Models;
using UnityEngine.Assertions;


namespace Meta.Controllers
{
    public class UnitModel
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


    /// <summary>
    /// Инвентарь для юнитов? Содержит в себе список юнитов
    /// При добавлении ориентируется на TypeUnitStacked - складывать:
    /// В одну кучу по типу юнита.
    /// В разные по типу юнита + прокачке 
    /// </summary>
    public interface IUnitsController
    {
        void Add(Id unitType, int count);
        void Add(Id unitType, UnitProgressionDto progression, int count);
        void Spend(UnitModel unitModel, int count);
        IEnumerable<UnitModel> GetUnits();
    }


    public class UnitsController : IUnitsController, IUnitsProgression
    {
        private readonly Dictionary<Id, UnitConfig> _configsHash;
        private readonly List<UnitDto> _unitsDto;
        private readonly Dictionary<UnitModel, UnitDto >_unitModels;
        private readonly IEqualityComparer<UnitProgressionDto> _progressionComparer = new UnitProgressionEqualsComparer();
        
        public UnitsController(List<UnitConfig> unitConfigs, List<UnitDto> unitsDto)
        {
            _unitsDto = unitsDto;
            _configsHash = new Dictionary<Id, UnitConfig>(unitConfigs.Count);
            _unitModels = new Dictionary<UnitModel, UnitDto>(_unitsDto.Count);
            
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

        //from factory?
        private UnitModel CreateUnitModel(UnitConfig config, UnitDto unitDto)
        {
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
            if (unitDto != null)
            {
                IncreaseUnitCount(unitDto, count);
            }
            else
            {
                AddNewUnit(config, progression, count);
            }
        }
        private void AddNotUpgradableUnit(UnitConfig config, UnitProgressionDto progression, int count)
        {
            var unitDto = _unitsDto.FirstOrDefault(s => s.UnitType.Equals(config.UnitType) && _progressionComparer.Equals(s.Progression, progression));
            if (unitDto != null)
            {
                IncreaseUnitCount(unitDto, count);
            }
            else
            {
                AddNewUnit(config, progression, count);
            }
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
        private void IncreaseUnitCount(UnitDto unit, int count)
        {
            unit.Count = checked(unit.Count + count);
        }

        public void Spend(UnitModel unitModel, int count)
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
        
        public IEnumerable<UnitModel> GetUnits()
        {
            return _unitModels.Keys.Where(s => s.Count > 0);
        }

        
        //--спорное решение держать этот здесь.
        public void Upgrade(UnitModel unit, TypeUnitStat stat)
        {
            //гредит юнита если не уперслись в лимиты
            throw new System.NotImplementedException();
        }

        public IEnumerable<UnitModel> GetCanUpgradeUnits()
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