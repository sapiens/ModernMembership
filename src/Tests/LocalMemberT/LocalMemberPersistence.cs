using ModernMembership;
using Ploeh.AutoFixture;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.LocalMemberT
{
    public class LocalMemberPersistence
    {
        private Stopwatch _t = new Stopwatch();

        public LocalMemberPersistence()
        {
            
        }

        [Fact]
        public void uses_a_memento()
        {
            var m = NewLocalMemberShould.CreateMember();
            var state = m.GetMemento();
            state.Id.Should().Be(m.Id);
            state.LoginId.Should().Be(m.LoginId.Value);
            state.Password.Should().Be(m.Password);
            state.Email.Should().Be(m.Email.Value);
            state.Status.Should().Be(m.Status);
            state.DisplayName.Should().Be(m.DisplayName);
            state.RegisteredOn.Should().Be(m.RegisteredOn);
        }

        public static LocalMemberMemento CreateMemento()
        {
            var m = Setup.GetAutoFixture().Create<LocalMemberMemento>();
            m.Id = Guid.NewGuid();
            m.Status=MemberStatus.Locked;
            m.Email = "bla@me.com";
            return m;
        }

        [Fact]
        public void state_restored_by_a_memento()
        {
            var m = CreateMemento();
            var member = LocalMember.RestoreFrom(m);
            member.Id.Should().Be(m.Id);
            member.LoginId.Value.Should().Be(m.LoginId);
            member.Password.Should().Be(m.Password);
            member.Email.Value.Should().Be(m.Email);
            member.DisplayName.Should().Be(m.DisplayName);
            member.Status.Should().Be(m.Status);
            member.RegisteredOn.Should().Be(m.RegisteredOn);
        }

        [Fact]
        public void restoration_doesnt_generate_events()
        {
            var m = LocalMember.RestoreFrom(CreateMemento());
            m.GetGeneratedEvents().Should().BeEmpty();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }

   
}