using System.Text.RegularExpressions;
using CavemanTools.Model.ValueObjects;
using System;
using System.Linq;

namespace ModernMembership
{
    public class LoginName:AbstractValueObject<string>,IEquatable<LoginName>
    {
        public LoginName(string value) : base(value)
        {
        }
        public const int MaxLength = 50;
        
        public static LoginName CreateRandomTestValue()
        {
            return new LoginName("test" + Guid.NewGuid().ToString().Substring(4));
        }

        protected override bool Validate(string value)
        {
            return IsValid(value);
        }

        /// <summary>
        /// Length between 3 and 50, starting with a letter or _, followed by letters/digits/_/-
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(string value)
        {
            return !value.IsNullOrEmpty() && value.Length>=3 && value.Length <= 50 && Regex.IsMatch(value, @"^[a-z_]+[\w\-]+$",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(LoginName other)
        {
            return other != null && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals((LoginName)obj);
        }
    }
}