using AppRen;

namespace Meta.Models
{
    public class ItemDto
    {
        public readonly Id ItemType;
        public int Count;

        /// <summary>
        /// if int.MaxValue - no limit
        /// </summary>
        public int Limit;

        public ItemDto(Id itemType)
        {
            ItemType = itemType;
        }

        public override int GetHashCode()
        {
            return ItemType.GetHashCode();
        }
    }
}