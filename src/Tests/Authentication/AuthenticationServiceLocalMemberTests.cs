using CavemanTools.Web;
using FakeItEasy;
using ModernMembership;
using ModernMembership.Authorization;
using Ploeh.AutoFixture;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.Authentication
{
    public class AuthenticationServiceLocalMemberTests
    {
        private Stopwatch _t = new Stopwatch();
        private AuthenticationService _sut;
        private readonly ILocalMembersRepository _repo;

        public AuthenticationServiceLocalMemberTests()
        {
            _repo = A.Fake<ILocalMembersRepository>();
            _sut = new AuthenticationService(_repo);
        }

        LocalMember SetupMember(string name,MemberStatus state=MemberStatus.Active)
        {
            var init = new LocalMember.Memento()
                {
                    Id = Guid.NewGuid()
                    ,LoginId = new LoginName(name)
                    ,Email = Setup.SomeEmail
                    ,Password = Setup.APassword.Hash
                    ,DisplayName = name+" display"
                    ,Status = state
                };
            return new LocalMember(init);
        }

        [Fact]
        public void authenticating_an_existing_active_member_succeeds()
        {
            A.CallTo(() => _repo.GetMember(new LoginName("existent"))).Returns(SetupMember("existent"));
            var result = _sut.Authenticate("existent", Setup.APassword.Value);
            result.Status.Should().Be(MemberSessionInfoStatus.Authenticated);
            result.DisplayName.Should().Be("existent display");
        }


        [Fact]
        public void authenticating_an_existing_active_member_with_wrong_password_returns_failed_pwd_state()
        {
            A.CallTo(() => _repo.GetMember(new LoginName("existent"))).Returns(SetupMember("existent"));
            var result=_sut.Authenticate("existent", "wrong pwd");
            result.Status.Should().Be(MemberSessionInfoStatus.PasswordFailed);
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void authenticating_an_existing_non_active_member_returns_session_member_inactive()
        {
            A.CallTo(() => _repo.GetMember(new LoginName("existent"))).Returns(SetupMember("existent",MemberStatus.Locked));
            var result=_sut.Authenticate("existent", Setup.APassword.Value);
            result.Status.Should().Be(MemberSessionInfoStatus.MemberInactive);
            result.MemberStatus.Should().Be(MemberStatus.Locked);
        }


        [Fact]
        public void authenticating_a_non_existing_member_returns_null()
        {
            _sut.Authenticate("someone", Setup.APassword.Value).Should().BeNull();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}