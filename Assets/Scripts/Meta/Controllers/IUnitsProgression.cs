using System.Collections.Generic;

namespace Meta.Controllers
{
    /// <summary>
    /// Менеджер прокачки юнитов?
    /// </summary>
    public interface IUnitsProgression
    {
        /// <summary>
        /// Прокачать юнита
        /// </summary>
        /// <param name="unit">кого качаем</param>
        /// <param name="stat">что качаем</param>
        void Upgrade(UnitModel unit, TypeUnitStat stat);
        
        /// <summary>
        /// </summary>
        /// <returns>Список доступных дляпрокачки юнитов</returns>
        IEnumerable<UnitModel> GetCanUpgradeUnits();
    }
}