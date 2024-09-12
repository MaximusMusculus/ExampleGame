using System.Collections.Generic;
using AppRen;
using Meta.Configs;

namespace Meta.Controllers
{
    public class UnitModel
    {
        //archyType 
        //archyTypeConfig?
        //progression
        //count?
        //сравнение по типу и прогрессии.
        public UnitConfig Config { get; } //??
        
        public Id UnitType { get; }
        public UnitProgressionModel Progression { get; }
        public int Count { get; }
        
    }

    public class UnitProgressionModel
    {
        
    }
    
    
    
    public interface IUnitProgression
    {
        //void UpgradeUnit(Unit unit, iUpgrade upgrade);
        void UpgradeMeleeAttack(UnitModel unitModel);
        void UpgradeRangedAttack(UnitModel unitModel);
        void UpgradeHealth(UnitModel unitModel);
    }

    public interface IUnitsController
    {
        void Add(UnitModel unitModel, int count);
        void Spend(UnitModel unitModel, int count);
        IEnumerable<UnitModel> GetUnits();
    }

    
}