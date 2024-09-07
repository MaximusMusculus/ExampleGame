using System.Collections.Generic;
using Application;

namespace Meta.Configs
{
    public class MetaConfig
    {
        public int ConfigVersion;
        //данные, что показывет интерфейс игрока (например счетчик ресурсов) тоже формируются и модифицируются в конфиге...
        public List<UnitConfig> Units = new List<UnitConfig>();
        public List<ResourceConfig> Resources = new List<ResourceConfig>();
    }
    
    
    /// <summary>
    /// Конфиг юнита позволяет расчитать его основные характеристики исходя из его прогресии (UnitProgressionDto)
    /// Melee, Range, Health
    /// Perks?
    /// </summary>
    public class UnitConfig
    {
        public Id UnitType;
        //nameKey
        //Icon?
    }
    
    public class ResourceConfig
    {
        public Id ResourceType;
        //nameKey
        //Icon?
    }
      
    
    
}