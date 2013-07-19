using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using ModernMembership;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.LocalMemberT
{
    public class NewLocalMemberShould
    {
        private Stopwatch _t = new Stopwatch();

      
        public static LocalMember CreateMember(LocalMember.Memento init=null)
        {
            LocalMember m;
            if (init == null)
            {
                 m= new LocalMember(Guid.NewGuid(), new LoginName("test"), new PasswordHash("bla"), new Email("bla@yahoo.com"),new ScopeId(Guid.NewGuid()));
            }
            else
            {
                m = new LocalMember(init);
            }
            return m;
        }

      
        [Fact]
        public void require_name_password_emailand_scope()
        {
            var m = new LocalMember(Guid.NewGuid(),new LoginName("test"), new PasswordHash("bla"), new Email("bla@yahoo.com"),new ScopeId(Guid.NewGuid()));
            m.LoginId.Should().NotBeNull();
            m.Password.Should().NotBeNull();
            m.Email.Should().NotBeNull();
            m.Scope.Should().NotBeNull();
        }

        [Fact]
        public void need_activation()
        {
            var m = CreateMember();
            m.Status.Should().Be(MemberStatus.NeedsActivation);
        }

        [Fact]
        public void not_have_a_display_name()
        {
            var m = CreateMember();
            m.DisplayName.Should().BeNullOrEmpty();
        }

        [Fact]
        public void have_not_the_default_date()
        {
            CreateMember().RegisteredOn.Should().NotBe(new DateTime());
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}