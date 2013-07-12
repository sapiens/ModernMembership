using CavemanTools.Model.ValueObjects;
using FluentAssertions;
using ModernMembership;
using Ploeh.AutoFixture;
using Xunit;
using System;
using System.Diagnostics;

namespace Tests.ExternalMemberT
{
    public class ExternalMemberTests
    {
        private Stopwatch _t = new Stopwatch();

        public ExternalMemberTests()
        {

        }

        public static ExternalMember Create()
        {
            return new ExternalMember(Guid.NewGuid(),new ExternalMemberId("fdb","23"));
        }

        [Fact]
        public void to_be_created_requires_local_and_external_id()
        {
            var id = Guid.NewGuid();
            var member = new ExternalMember(id, new ExternalMemberId("fb", "12"));
            member.Id.Should().Be(id);
            member.ExternalId.Should().Be(new ExternalMemberId("fb", "12"));
        }

        [Fact]
        public void at_creation_optional_properties_are_null()
        {
            var m = Create();
            m.DisplayName.Should().BeNull();
        }


        [Fact]
        public void can_accept_displayname_at_creation()
        {
            var m = new ExternalMember(Guid.NewGuid(), Setup.GetAutoFixture().Create<ExternalMemberId>(),displayName:"hi");
            m.DisplayName.Should().Be("hi");
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}