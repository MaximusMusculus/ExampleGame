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
    /// Тип конфига может быть полиморфным.
    /// Т.е. тут может быть формула с параметрами, а может табличная часть, да все что угодно. 
    /// </summary>
    public abstract class UnitConfig
    {
        public Id UnitType;
        //nameKey
        //Icon?
        //public abstract UnitPower Calculate(UnitProgression);
    }

    public class UnitConfigFromFormula : UnitConfig
    {
        //calc from Formula
        //Formula for Concrete Info for concrete Unit
        
    }

    public class UnitConfigFromTable : UnitConfig
    {
        //calc from Table Data
        //TableData for Concrete Unit
        public List<int> Melee;
        public List<int> Range;
        public List<int> Health;
        //public override UnitPower Calculate(UnitProgression)
    }



    public class ResourceConfig
    {
        public Id ResourceType;
        //nameKey
        //Icon?
    }
      
    
    
}