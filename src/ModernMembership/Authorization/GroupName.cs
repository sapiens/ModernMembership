using System.Text.RegularExpressions;
using CavemanTools.Model.ValueObjects;
using System;

namespace ModernMembership.Authorization
{
    public class GroupName:AbstractValueObject<string>
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
    }
}