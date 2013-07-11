using System.Text.RegularExpressions;
using CavemanTools.Model.ValueObjects;
using System;

namespace ModernMembership
{
    public class LoginName:AbstractValueObject<string>
    {
        public LoginName(string value) : base(value)
        {
        }
        public const int MaxLength = 50;
        
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
    }
}