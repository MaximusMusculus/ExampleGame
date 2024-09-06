

namespace Application.Id
{
    public interface IIdProvider
    {
        Id GetNext();
        //void Return(Id id);
    }

    public class IDProviderData
    {
         public Id NextId;
    } 
        
    public class IdProvider : IIdProvider
    {
        private readonly IDProviderData _data;

        public IdProvider(IDProviderData data)
        {
            _data = data;
        }
        
        public Id GetNext()
        {
            return _data.NextId++;
        }
    }



}
