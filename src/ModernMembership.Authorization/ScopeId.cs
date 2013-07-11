using System;
using CavemanTools.Model.ValueObjects;

namespace ModernMembership
{
    public class ScopeId:AbstractValueObject<Guid>,IEquatable<ScopeId>
    {
        public ScopeId(Guid value) : base(value)
        {
        }

        protected override bool Validate(Guid value)
        {
            return true;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ScopeId other)
        {
            if (other == null) return false;
            return _value == other._value;
        }
    }
}