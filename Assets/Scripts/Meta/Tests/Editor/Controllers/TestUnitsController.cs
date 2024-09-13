using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Models;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class UnitsControllerTests
    {
        private  UnitsController _upgradeController; 
        private IUnitsController _unitsController;
        private List<UnitConfig> _unitConfigs;
        private List<UnitDto> _unitsDto;

        private readonly Id _upgradableUnit = 1;
        private readonly Id _notUpgradableUnit = 2;
        
        private readonly Id _existUpgradableUnit = 3;
        private readonly Id _existNotUpgradableUnit = 4;
        
        [SetUp]
        public void Setup()
        {
            _unitsDto = new List<UnitDto>
            {
                new UnitDto {UnitType = _existUpgradableUnit, Count = 5, Progression = new UnitProgressionDto {HealthLevel = 5}},
                new UnitDto {UnitType = _existNotUpgradableUnit, Count = 5, Progression = new UnitProgressionDto {MeleeAttackLevel = 5}}
            };
            _unitConfigs = new List<UnitConfig>
            {
                new UnitConfig {UnitType = _upgradableUnit, IsCanUpgrade = true},
                new UnitConfig {UnitType = _notUpgradableUnit, IsCanUpgrade = false},
                new UnitConfig {UnitType = _existUpgradableUnit, IsCanUpgrade = true},
                new UnitConfig {UnitType = _existNotUpgradableUnit, IsCanUpgrade = false}
            };
            _unitsController = new UnitsController(_unitConfigs, _unitsDto);
            
            //unitBarraсks <- unitConfigs+Prices+Conditions? 
        }

        [Test]
        public void Add_UpgradableUnit_WhenAddingNewUnit()
        {
            _unitsController.Add(_upgradableUnit, 1);
            var unitDto = _unitsDto.First(u => u.UnitType.Equals(_upgradableUnit));
            Assert.AreEqual(1, unitDto.Count);
        }
        
        [Test]
        public void Add_UpgradableUnit_WhenAddingExistingUnit()
        {
            _unitsController.Add(_upgradableUnit, 1);
            _unitsController.Add(_upgradableUnit, 1);
            var unitDto = _unitsDto.First(u => u.UnitType.Equals(_upgradableUnit));
            Assert.AreEqual(2, unitDto.Count);
        }

        [Test]
        public void Add_NotUpgradableUnit_WhenAddingNewUnit()
        {
            _unitsController.Add(_notUpgradableUnit, new UnitProgressionDto {HealthLevel = 1}, 1);
            var unitDto = _unitsDto.First(u => u.UnitType.Equals(_notUpgradableUnit));
            Assert.AreEqual(1, unitDto.Count);
        }
        
        
        [Test]
        public void Add_NotUpgradableUnit_WhenAddingDifferentProgression()
        {
            _unitsController.Add(_notUpgradableUnit, new UnitProgressionDto {HealthLevel = 1}, 1);
            _unitsController.Add(_notUpgradableUnit, new UnitProgressionDto {HealthLevel = 2}, 1);
            var unitDto = _unitsDto.First(u => u.UnitType.Equals(_notUpgradableUnit));
            Assert.AreEqual(1, unitDto.Count);
        }

        [Test]
        public void Add_NotUpgradableUnit_WhenAddingSameProgression()
        {
            _unitsController.Add(_notUpgradableUnit, new UnitProgressionDto {HealthLevel = 1}, 1);
            _unitsController.Add(_notUpgradableUnit, new UnitProgressionDto {HealthLevel = 1}, 1);
            var unitDto = _unitsDto.First(u => u.UnitType.Equals(_notUpgradableUnit));
            Assert.AreEqual(2, unitDto.Count);
        }

         [Test]
        public void Spend_UpgradableUnit_WhenSpendingCount()
        {
            var unitModel = _unitsController.GetUnits().First(u => u.UnitType.Equals(_existUpgradableUnit));
            _unitsController.Spend(unitModel, 3);
            Assert.AreEqual(2, unitModel.Count);
        }
        
        [Test]
        public void Spend_UpgradableUnit_WhenSpendingAllAvailable_ReturnsNoUnits()
        {
            var unitModel = _unitsController.GetUnits().First(u => u.UnitType.Equals(_existUpgradableUnit));
            _unitsController.Spend(unitModel, 5);
            
            Assert.IsFalse(_unitsController.GetUnits().Any(u => u.UnitType.Equals(_existUpgradableUnit)));
            Assert.IsTrue(_unitsDto.Any(u => u.UnitType.Equals(_existUpgradableUnit)));
        }
        
        [Test]
        public void Spend_NotUpgradableUnit_WhenSpendingCount()
        {
            var unitModel = _unitsController.GetUnits().First(u => u.UnitType.Equals(_existNotUpgradableUnit));
            _unitsController.Spend(unitModel, 3);
            Assert.AreEqual(2, unitModel.Count);
            Assert.AreEqual(2, _unitsDto.First(u => u.UnitType.Equals(_existNotUpgradableUnit)).Count);
        }
        
        [Test]
        public void Spend_NotUpgradableUnit_WhenSpendingAllAvailable_ReturnsNoUnits()
        {
            var unitModel = _unitsController.GetUnits().First(u => u.UnitType.Equals(_existNotUpgradableUnit));
            _unitsController.Spend(unitModel, 5);
            Assert.IsFalse(_unitsDto.Any(u => u.UnitType.Equals(_existNotUpgradableUnit)));
            Assert.IsFalse(_unitsController.GetUnits().Any(u => u.UnitType.Equals(_existNotUpgradableUnit)));
        }
        
        
        [Test]
        public void Spend_NotUpgradableUnit_WhenSpendingMoreAvailable_ThrowException()
        {
            var unitModel = _unitsController.GetUnits().First(u => u.UnitType.Equals(_existNotUpgradableUnit));
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => _unitsController.Spend(unitModel, 6));
            
            Assert.IsTrue(_unitsDto.Any(u => u.UnitType.Equals(_existNotUpgradableUnit)));
            Assert.IsTrue(_unitsController.GetUnits().Any(u => u.UnitType.Equals(_existNotUpgradableUnit)));
        }

    }
}