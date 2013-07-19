using System.Linq;
using CavemanTools.Model.ValueObjects;
using FluentAssertions;
using ModernMembership;
using ModernMembership.Events;
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
            return new ExternalMember(Guid.NewGuid(),new ExternalMemberId("fdb","23"), ScopeId.None);
        }

        [Fact]
        public void to_be_created_requires_local_external_id_and_scope()
        {
            var id = Guid.NewGuid();
            var scopeId = new ScopeId(Guid.NewGuid());
            var member = new ExternalMember(id, new ExternalMemberId("fb", "12"), scopeId);
            member.Id.Should().Be(id);
            member.ExternalId.Should().Be(new ExternalMemberId("fb", "12"));
            member.Scope.Should().Be(scopeId);
        }

        [Fact]
        public void at_creation_optional_properties_are_null()
        {
            var m = Create();
            m.DisplayName.Should().BeNull();
            m.GetGeneratedEvents().Should().BeEmpty();
        }


        [Fact]
        public void can_accept_displayname_at_creation()
        {
            var m = new ExternalMember(Guid.NewGuid(), Setup.GetAutoFixture().Create<ExternalMemberId>(), ScopeId.None,displayName: "hi");
            m.DisplayName.Should().Be("hi");
        }

        [Fact]
        public void default_state_is_active()
        {
            var member = Create();
            member.Status.Should().Be(MemberStatus.Active);
        }

        [Fact]
        public void changing_state_generates_event()
        {
            var member = Create();
            member.Status=MemberStatus.Banned;
            member.GetGeneratedEvents().First().Cast<MemberStatusChanged>().Status.Should().Be(MemberStatus.Banned);
        }


        [Fact]
        public void restore_member_doesnt_generates_events()
        {
            var member = new ExternalMember(Guid.NewGuid(), Setup.GetAutoFixture().Create<ExternalMemberId>(), ScopeId.None,
                                            displayName: "display name", status: MemberStatus.Locked);
            member.GetGeneratedEvents().Should().BeEmpty();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}