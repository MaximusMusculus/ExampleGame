using AppRen;

namespace Meta.Models
{
    public class UnitDto
    {
        public Id UnitType;
        public int Count;
        public UnitProgressionDto Progression;

        //Возможно для определения хеша стейта игрока, нужен другой свой метод. Так как базовый  используется для работы со словарем или хеш сетом
        public override int GetHashCode() => HashHelper.GetHashCode(UnitType, Count, Progression);
    }
}