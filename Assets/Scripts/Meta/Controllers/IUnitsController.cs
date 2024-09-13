using System.Collections.Generic;
using AppRen;
using Meta.Models;

namespace Meta.Controllers
{
    public interface IUnitModel
    {
        //public UnitConfig Config { get; } //??
        public Id UnitType { get; }
        public UnitStatsModel Stats { get; }
        public int Count { get; }
    }
    
    /// <summary>
    /// Менеджер прокачки юнитов?
    /// </summary>
    public interface IUnitsProgression
    {
        /// <summary>
        /// Прокачать юнита
        /// </summary>
        /// <param name="unitType">кого качаем</param>
        /// <param name="stat">что качаем</param>
        void Upgrade(Id unitType, TypeUnitStat stat);
        
        /// <summary>
        /// </summary>
        /// <returns>Список доступных дляпрокачки юнитов</returns>
        IEnumerable<IUnitModel> GetCanUpgradeUnits();
    }
    
    
    /// <summary>
    /// При добавлении ориентируется на IsCanUpgrade -
    /// То, что грейдится, складывается в одну кучу. И не удаляется при 0.
    /// </summary>
    public interface IUnitsController
    {
        void Add(Id unitType, int count);
        void Add(Id unitType, UnitProgressionDto progression, int count);
        void Spend(IUnitModel unitModel, int count);
        
        IEnumerable<IUnitModel> GetUnits();
    }
}