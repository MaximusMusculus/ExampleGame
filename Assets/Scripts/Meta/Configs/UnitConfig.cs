using System.Collections.Generic;
using Application;

namespace Meta.Configs
{
    /// <summary>
    /// Конфиг юнита позволяет расчитать его основные характеристики исходя из его прогресии (UnitProgressionDto)
    /// Melee, Range, Health
    /// Тип конфига может быть полиморфным.
    /// Т.е. тут может быть формула с параметрами, а может табличная часть, да все что угодно. 
    /// </summary>
    public abstract class UnitConfig
    {
        public Id UnitType;
        public abstract TypeUnitCalculation TypeCalculation { get; }
    }
    
    
   /// <summary>
   /// В моделе игры по TypeUnitCalculation выбирается конкретный UnitCalculationController с методами в которые передается UnitProgressionDto
   /// и получается UnitPowerInof и все другое, что мне захочется. Притом, конфиг не будет знать про Dto и про PowerInfo  это классы данных и игры  
   /// </summary>
    public enum TypeUnitCalculation
    {
        Formula,
        Table
    }
    
    public class UnitConfigFromFormula : UnitConfig
    {
        public override TypeUnitCalculation TypeCalculation=> TypeUnitCalculation.Formula;
    }
    
    public class UnitConfigFromTable : UnitConfig
    {
        public override TypeUnitCalculation TypeCalculation => TypeUnitCalculation.Table;
        
        public List<int> Melee;
        public List<int> Range;
        public List<int> Health;
    }
    


}