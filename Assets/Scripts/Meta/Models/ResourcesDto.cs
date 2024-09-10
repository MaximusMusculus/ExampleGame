using AppRen;

namespace Meta.Models
{
    public class ResourcesDto
    {
        public Id ResourceType;
        public int Count;
        public int Limit;

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(ResourceType, Count, Limit);
        }
    }
}