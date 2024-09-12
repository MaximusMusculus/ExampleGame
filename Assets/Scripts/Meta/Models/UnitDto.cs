using AppRen;

namespace Meta.Models
{
    public class UnitDto
    {
        public Id UnitType;
        public UnitProgressionDto Progression;
        
        /// <summary>
        /// у нас может быть несколько юнитов одного типа, но с разной прокачкой
        /// не хочтся делать доп id индексы, поэтому количество и прокачка юнита в одном месте
        /// </summary>
        public int Count;
    }
}