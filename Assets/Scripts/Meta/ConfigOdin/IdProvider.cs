using Sirenix.OdinInspector;

namespace Meta.ConfigOdin
{
    public interface IHashCode
    {
        int GetHashCode();
    }
    public class IdProviderDto : IHashCode
    {
        [ReadOnly] public ulong NextId;

        public override int GetHashCode()
        {
            return NextId.GetHashCode();
        }
    }
    
    
    public class IdReadOnlyDto
    {
        private readonly IdProviderDto _idProviderDto;
        public ulong NextId => _idProviderDto.NextId;

        public IdReadOnlyDto(IdProviderDto idProviderDto)
        {
            _idProviderDto = idProviderDto;
            
        }
    }

    public interface IIdProvider
    {
        ulong GetNext();
    }

    public class IdProvider : IIdProvider
    {
        private readonly IdProviderDto _idProviderDto;

        public IdProvider(IdProviderDto idProviderDto)
        {
            _idProviderDto = idProviderDto;
        }

        public ulong GetNext()
        {
            return _idProviderDto.NextId++;
        }
    }
}