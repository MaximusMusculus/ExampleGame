using System;

namespace Meta.Configs.Conditions
{
    public enum TypeCompare
    {
        None,
        Greater, //>
        GreaterOrEqual, //>=
        Less, //<
        LessOrEqual, //<=
        Equal, //==
        NotEqual, //!=
    }
    
    public static class UtilCompareExtended
    {
        private static readonly float FloatingPointComparisonTolerance = 0.01f;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyValue">Реальное значение</param>
        /// <param name="typeCompare">Операция сравнения</param>
        /// <param name="compareValue">Значение из конфига</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool CheckCompareIsTrue(this int propertyValue, TypeCompare typeCompare, int compareValue)
        {
            return typeCompare switch
            {
                TypeCompare.Greater => propertyValue > compareValue,
                TypeCompare.GreaterOrEqual => propertyValue >= compareValue,
                TypeCompare.Less => propertyValue < compareValue,
                TypeCompare.LessOrEqual => propertyValue <= compareValue,
                TypeCompare.Equal => propertyValue == compareValue,
                TypeCompare.NotEqual => propertyValue != compareValue,
                
                _ => throw new ArgumentOutOfRangeException(nameof(typeCompare), typeCompare, null)
            };
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyValue">Реальное значение</param>
        /// <param name="typeCompare">Операция сравнения</param>
        /// <param name="compareValue">Значение из конфига</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool CheckCompareIsTrue(this float propertyValue, TypeCompare typeCompare, float compareValue)
        {
            return typeCompare switch
            {
                TypeCompare.Greater => propertyValue > compareValue,
                TypeCompare.GreaterOrEqual => propertyValue >= compareValue,
                TypeCompare.Less => propertyValue < compareValue,
                TypeCompare.LessOrEqual => propertyValue <= compareValue,
                TypeCompare.Equal => Math.Abs(propertyValue - compareValue) < FloatingPointComparisonTolerance,
                TypeCompare.NotEqual => Math.Abs(propertyValue - compareValue) > FloatingPointComparisonTolerance,
                _ => throw new ArgumentOutOfRangeException(nameof(typeCompare), typeCompare, null)
            };
        }
    }
}