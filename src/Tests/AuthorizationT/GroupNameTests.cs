using System;
using FluentAssertions;
using ModernMembership.Authorization;
using Xunit;
using Xunit.Extensions;

namespace Tests.Authorization
{
    public class GroupNameTests
    {
        [Fact]
        public void name_must_contain_only_letters_space_underscores_and_digits()
        {
            var name = new GroupName("group 1");
            name.Value.Should().Be("group 1");
        }

        [Fact]
        public void name_must_not_be_empty()
        {
            Assert.Throws<ArgumentException>(() => new GroupName(""));
        }

        [Theory]
        [InlineData("gr")]
        [InlineData("a1234567891234567890123456789012345678901234567890d")]
        public void length_is_between_3_and_50(string data)
        {
            Assert.Throws<ArgumentException>(() => new GroupName(data));
            GroupName.IsValid(data).Should().BeFalse();
        }
    }
}