using AppRen;

namespace Meta.Models
{
    public class ItemDto
    {
        public Id Id;
        public int Count;

        /// <summary>
        /// if int.MaxValue - no limit
        /// </summary>
        public int Limit;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}