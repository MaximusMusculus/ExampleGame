using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Models;
using UnityEngine.Assertions;

namespace Meta.Controllers.Imp
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
        
        private readonly Dictionary<IUnitModel, UnitDto> _unitsModels;
        private readonly IEqualityComparer<UnitProgressionDto> _progressionComparer = new UnitProgressionEqualsComparer();

        public UnitsController(List<UnitConfig> unitConfigs, List<UnitDto> unitsDto)
        {
            _unitsDto = unitsDto;
            _configsHash = new Dictionary<Id, UnitConfig>(unitConfigs.Count);
            _unitsModels = new Dictionary<IUnitModel, UnitDto>(_unitsDto.Count);

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
                _unitsModels.Add(CreateUnitModel(config, unitDto), unitDto);
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
            progression ??= config.GetDefaultProgression();  //??
                
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
            UnitDto unitDto = null;
            foreach (var dto in _unitsDto)
            {
                if(dto.UnitType.Equals(config.UnitType))
                {
                    unitDto = dto;
                    break;
                }
            }
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
            _unitsModels.Add(CreateUnitModel(config, unit), unit);
        }


        public void Spend(IUnitModel unitModel, int count)
        {
            Assert.IsTrue(unitModel.Count >= count);

            var dto = _unitsModels[unitModel];
            var config = _configsHash[unitModel.UnitType];
            dto.Count -= count;

            if (dto.Count > 0 || config.IsCanUpgrade)
            {
                return;
            }

            _unitsDto.Remove(dto);
            _unitsModels.Remove(unitModel);
        }


        public bool TryGetUnit(Id typeUnit, UnitProgressionDto progression, out IUnitModel model)
        {
            //Получить юнита с соотв прокачкой.
            _unitKey.UnitId = typeUnit;
            _unitKey.Progression = progression;
            _unitKey.Reset();
            
            
            //растет o(n), станет критичено - поправим.
            model = null;
            foreach (var unitsModel in _unitsModels)
            {
                if (unitsModel.Key.UnitType.Equals(typeUnit) && _progressionComparer.Equals(unitsModel.Value.Progression, progression))
                {
                    model = unitsModel.Key;
                    return true;
                }
            }
            return false;
        }
        public IEnumerable<IUnitModel> GetUnits()
        {
            foreach (var unitModel in _unitsModels.Keys)
            {
                if (unitModel.Count > 0)
                {
                    yield return unitModel;
                }
            }
        }
        
        
        //----- как оптимизация потом
        private UnitKey _unitKey = new UnitKey(0, new UnitProgressionDto(), new UnitProgressionEqualsComparer());
        private struct UnitKey : IEquatable<UnitKey>
        {
            public Id UnitId { get; set; }
            public UnitProgressionDto Progression { get; set; }

            public void Reset()
            {
                UnitId = 0;
                Progression = null;
            }

            private readonly UnitProgressionEqualsComparer _progressionComparer;

            public UnitKey(Id unitId, UnitProgressionDto progression, UnitProgressionEqualsComparer progressionComparer)
            {
                UnitId = unitId;
                Progression = progression;
                _progressionComparer = progressionComparer;
            }

            public override bool Equals(object obj) => obj is UnitKey key && Equals(key);

            public bool Equals(UnitKey other)
            {
                return UnitId.Equals(other.UnitId) && _progressionComparer.Equals(Progression, other.Progression);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(UnitId, Progression);
            }
        }

        


        //--спорное решение держать этот здесь.
        //учитывая, что грейдятся только сингл юниты, то можно по типу дергать.
        public void Upgrade(Id unitType, TypeUnitStat stat)
        {
            var config = _configsHash[unitType];
            Assert.IsTrue(config.IsCanUpgrade);
            var unitModel = _unitsModels.Keys.FirstOrDefault(s => s.UnitType == unitType);
            if (unitModel == null)
            {
                throw new InvalidOperationException($"Unit:{unitType} not found model");
            }
            var unitDto = _unitsModels[unitModel];
            var unitProgressionDto = unitDto.Progression;
            //тут получаем цену от модели калькулятора. 

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
            unitModel.Stats.UpgradeHandler();
        }
        public IEnumerable<IUnitModel> GetCanUpgradeUnits() //IUnitModel->IUnitUpgradeModel? или из спец менеджера калькулятора?
        {
            foreach (var model in _unitsModels)
            {
                if (_configsHash[model.Key.UnitType].IsCanUpgrade)
                {
                    yield return model.Key;
                }
            }
        }
    }
    
}