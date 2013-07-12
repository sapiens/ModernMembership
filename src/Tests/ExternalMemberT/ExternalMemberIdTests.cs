using ModernMembership;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit.Extensions;

namespace Tests.ExternalMemberT
{
    public class ExternalMemberIdTests
    {
        private Stopwatch _t = new Stopwatch();

        public ExternalMemberIdTests()
        {

        }

        [Fact]
        public void requires_external_prefix_and_external_id()
        {
            var id = new ExternalMemberId("fb", "34");
            id.ProviderPrefix.Should().Be("fb");
            id.Value.Should().Be("34");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void prefix_cant_be_empty_or_spaces(string prefix)
        {
            Assert.Throws<FormatException>(() => new ExternalMemberId(prefix, "34"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void id_value_cant_be_empty(string id)
        {
            Assert.Throws<FormatException>(() => new ExternalMemberId("fb", id));
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}