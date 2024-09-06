using System;

namespace Application.Id
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
}