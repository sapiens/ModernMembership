using System.Text.RegularExpressions;
using CavemanTools.Model.ValueObjects;
using System;

namespace ModernMembership.Authorization
{
    public class GroupName:AbstractValueObject<string>,IEquatable<GroupName>
    {
        public GroupName(string value) : base(value)
        {
        }

        public const int MaxLength = 50;
        public const int MinLength = 3;

        protected override bool Validate(string value)
        {
            return IsValid(value);
        }

        public static bool IsValid(string value)
        {
            return !value.IsNullOrEmpty() && (value.Length>=MinLength && value.Length<=MaxLength) && Regex.IsMatch(value, @"^[a-z]+[\w ]*?$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(GroupName other)
        {
            if (other == null) return false;
            return other.Value == Value;
        }

        public override bool Equals(object obj)
        {
            return Equals((GroupName)obj);
        }
        
    }
}