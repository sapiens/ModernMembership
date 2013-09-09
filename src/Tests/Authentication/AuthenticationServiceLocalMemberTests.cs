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
            A.CallTo(() => _repo.GetMember(new LoginName("existent"), ScopeId.Global)).Returns(SetupMember("existent"));
        }

        LocalMember SetupMember(string name,MemberStatus state=MemberStatus.Active,ScopeId scope=null)
        {
            var init = new LocalMember.Memento()
                {
                    Id = Guid.NewGuid()
                    ,LoginId = new LoginName(name)
                    ,Email = Setup.SomeEmail
                    ,Password = Setup.APassword.Hash
                    ,DisplayName = name+" display"
                    ,Status = state
                    ,Scope = scope
                };
            return new LocalMember(init);
        }

        [Fact]
        public void authenticating_an_existing_active_member_returns_active_session()
        {
            var result = _sut.Authenticate("existent", Setup.APassword.Value);
            result.Status.Should().Be(MemberSessionInfoStatus.Authenticated);
            result.DisplayName.Should().Be("existent display");
            result.MemberStatus.Should().Be(MemberStatus.Active);
            result.Scope.Should().Be(ScopeId.Global);
        }


        [Fact]
        public void authenticating_an_existing_active_member_with_wrong_password_returns_failed_pwd_session()
        {
            var result=_sut.Authenticate("existent", "wrong pwd");
            result.Status.Should().Be(MemberSessionInfoStatus.PasswordFailed);
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void authenticating_an_existing_non_active_member_returns_inactive_session()
        {
            A.CallTo(() => _repo.GetMember(new LoginName("existent"),ScopeId.Global)).Returns(SetupMember("existent",MemberStatus.Locked));
            var result=_sut.Authenticate("existent", Setup.APassword.Value);
            result.Status.Should().Be(MemberSessionInfoStatus.MemberInactive);
            result.MemberStatus.Should().Be(MemberStatus.Locked);
        }


        [Fact]
        public void authenticating_a_non_existing_member_returns_null()
        {
            _sut.Authenticate("someone", Setup.APassword.Value).Should().BeNull();
        }

        [Fact]
        public void authenticating_scoped_user_in_scope_returns_active_session()
        {
            var scope = Setup.GetAutoFixture().Create<ScopeId>();
            A.CallTo(() => _repo.GetMember(new LoginName("scoped"), scope)).Returns(SetupMember("scoped", scope:scope));
            var info = _sut.Authenticate("scoped", Setup.APassword.Value, scope: scope);
            info.Status.Should().Be(MemberSessionInfoStatus.Authenticated);
            info.MemberStatus.Should().Be(MemberStatus.Active);
            info.Scope.Should().Be(scope);
        }

        [Fact]
        public void authenticating_a_global_user_within_scope_returns_null()
        {
            _sut.Authenticate("existent", Setup.APassword.Value, scope: Setup.AScope).Should().BeNull();
        }
        
        [Fact]
        public void authenticating_a_scoped_user_in_global_scope_returns_null()
        {
            A.CallTo(() => _repo.GetMember(new LoginName("scoped"), Setup.AScope)).Returns(SetupMember("scoped", scope: Setup.AScope));
            _sut.Authenticate("scoped", Setup.APassword.Value, scope:ScopeId.Global).Should().BeNull();
        }



        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}