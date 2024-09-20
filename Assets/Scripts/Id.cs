using System;


public static class DefaultCapacityConst
{
    public const int Micro = 8;
    public const int Small = 16;
    public const int Medium = 32;
    public const int Large = 64;
}


namespace AppRen
{

    public struct Id : IEquatable<Id>, IFormattable, IComparable<Id>
    {
        private ushort _value;
        public ushort Value => _value;

        public Id(ushort value)
        {
            _value = value;
        }

        public static implicit operator Id(ushort value)
        {
            return new Id(value);
        }

        public static implicit operator ushort(Id id)
        {
            return id._value;
        }


        public override bool Equals(object obj) => obj is Id num && _value == num._value;

        public bool Equals(Id other) => _value == other._value;

        public override int GetHashCode() => _value;

        public int CompareTo(Id other) => _value.CompareTo(other._value);

        public override string ToString() => _value.ToString();

        public string ToString(string format, IFormatProvider formatProvider) => _value.ToString(format, formatProvider);

    }
    
    public interface IIdProvider
    {
        Id GetId();
        //void Return(Id id);
    }

    [Serializable]
    public class IdProviderDto
    {
        public ushort nextId;
    }

    public class IdProvider : IIdProvider
    {
        private readonly IdProviderDto _dto;

        public IdProvider(IdProviderDto dto)
        {
            _dto = dto;
        }

        public Id GetId()
        {
            return _dto.nextId++;
        }
    }
}