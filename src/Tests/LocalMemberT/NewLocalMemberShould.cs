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

        public NewLocalMemberShould()
        {

        }

        public static ModernMembership.LocalMember CreateMember()
        {
            var m = new ModernMembership.LocalMember(Guid.NewGuid(),new LoginName("test"), new PasswordHash("bla"), new Email("bla@yahoo.com"));
            return m;
        }

      
        [Fact]
        public void require_name_password_and_email()
        {
            var m = new ModernMembership.LocalMember(Guid.NewGuid(),new LoginName("test"), new PasswordHash("bla"), new Email("bla@yahoo.com"));
            m.LoginId.Should().NotBeNull();
            m.Password.Should().NotBeNull();
            m.Email.Should().NotBeNull();            
        }

        [Fact]
        public void need_activation()
        {
            var m = CreateMember();
            m.Status.Should().Be(MemberStatus.NeedsActivation);
        }

        [Fact]
        public void have_a_display_name()
        {
            var m = CreateMember();
            m.DisplayName.Should().Be(m.LoginId.Value);
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}