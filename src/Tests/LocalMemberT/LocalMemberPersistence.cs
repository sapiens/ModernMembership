using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
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
        public void memento_contains_member_state()
        {
            var m = NewLocalMemberShould.CreateMember();
            var state = m.GetMemento();
            state.Id.Should().Be(m.Id);
            state.LoginId.Should().Be(m.LoginId);
            state.Password.Should().Be(m.Password);
            state.Email.Should().Be(m.Email);
            state.Status.Should().Be(m.Status);
            state.DisplayName.Should().Be(m.DisplayName);
            state.RegisteredOn.Should().Be(m.RegisteredOn);
            state.Scope.Should().Be(m.Scope);
        }

        public static LocalMember.Memento CreateMemento()
        {
            var m = new LocalMember.Memento();              
            m.Id = Guid.NewGuid();
            m.LoginId=new LoginName("valid-login-name");
            m.Status=MemberStatus.Locked;
            m.Email = new Email("bla@me.com");
            m.Password=new PasswordHash("hash");
            return m;
        }

        [Fact]
        public void restored_member_contains_all_memento_data()
        {
            var m = CreateMemento();
            var member = new LocalMember(m);
            member.Id.Should().Be(m.Id);
            member.LoginId.Should().Be(m.LoginId);
            member.Password.Should().Be(m.Password);
            member.Email.Should().Be(m.Email);
            member.DisplayName.Should().Be(m.DisplayName);
            member.Status.Should().Be(m.Status);
            member.RegisteredOn.Should().Be(m.RegisteredOn);
            member.Scope.Should().Be(m.Scope);
        }

        [Fact]
        public void restoring_fails_for_invalid_memento_data()
        {
            var memento = CreateMemento();
            memento.Email = null;
            Assert.Throws<ArgumentNullException>(() => new LocalMember(memento));
        }

        [Fact]
        public void restoration_doesnt_generate_events()
        {
            var m = new LocalMember(CreateMemento());
            m.GetGeneratedEvents().Should().BeEmpty();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }

   
}