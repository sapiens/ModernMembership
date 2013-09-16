using System;

namespace ModernMembership
{
    public class ExternalMemberId:IEquatable<ExternalMemberId>
    {
        public string ProviderPrefix { get; private set; }
        public string Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerPrefix">Ex: fb, twitter</param>
        /// <param name="userId">Third party user id</param>
        public ExternalMemberId(string providerPrefix,string userId)
        {
            providerPrefix.MustNotBeEmpty();
            userId.MustNotBeEmpty();
            ProviderPrefix = providerPrefix;
            Value = userId;
        }

        public static ExternalMemberId RandomTestValue()
        {
            return new ExternalMemberId("tw",Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ExternalMemberId other)
        {
            return other != null && (ProviderPrefix == other.ProviderPrefix && Value == other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals((ExternalMemberId) obj);
        }

        public override int GetHashCode()
        {
            return ProviderPrefix.GetHashCode() + 17*Value.GetHashCode();
        }

        public override string ToString()
        {
            return ProviderPrefix + "-" + Value;
        }

    }
}