using System;
using System.Collections;
using System.Collections.Generic;
using AppRen;

namespace Meta.Configs.Conditions
{
    public enum TypeCompare
    {
        None,
        Greater,    //>
        GreaterOrEqual, //>=
        Less,       //<
        LessOrEqual,    //<=
        Equal,          //==
        NotEqual,       //!=
    }
    public static class UtilExtended
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
    
    
    public class ItemConditionConfig : IConditionConfig
    {
        public TypeCondition TypeCondition { get; set; }
        public Id TypeItem;
        public TypeCompare CompareType;
        public int Value;
    }
    
    public class ConditionCollectionConfig : IConditionConfig, IEnumerable<IConditionConfig>
    {
        public TypeCondition TypeCondition => TypeCollection;
        public TypeCondition TypeCollection; //And.Or.Not?
        public List<IConditionConfig> Conditions = new List<IConditionConfig>();
        
        public IEnumerator<IConditionConfig> GetEnumerator()
        {
            return Conditions.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}