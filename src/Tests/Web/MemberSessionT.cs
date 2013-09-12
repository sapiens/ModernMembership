using System.Collections.Generic;
using ModernMembership;
using ModernMembership.Authorization;
using ModernMembership.Web;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.Web
{
    public class MemberSessionT
    {
        private Stopwatch _t = new Stopwatch();
        private MemberSessionPrincipal _sut;

        public MemberSessionT()
        {
            var data = new SessionStorageData()
                {
                    Id = Setup.AnId,
                    MemberInfo = new MemberSessionData()
                        {
                            MemberId = Setup.AnId,
                            MemberName = "name",
                            Rights = new[] { new ScopedRights(Setup.AScope, new short[] { SetupUserRights.Right1, SetupUserRights.Right2 }) }
                        }
                };
            _sut = new MemberSessionPrincipal(data);
        }

        [Fact]
        public void authenticated_user_is_true()
        {
            _sut.IsAuthenticated.Should().BeTrue();
        }

        [Fact]
        public void session_has_unique_id()
        {
            _sut.Info.Id.Should().Be(Setup.AnId);
        }

        [Fact]
        public void authenticated_member_has_name()
        {
            _sut.Name.Should().Be("name");
        }

        [Fact]
        public void user_id_is_set()
        {
            _sut.MemberId.Should().Be(Setup.AnId);
        }

        [Fact]
        public void has_a_scope_that_can_be_set()
        {
            _sut.Scope = Setup.AScope;
            _sut.Scope.Should().Be(Setup.AScope);
        }

        [Fact]
        public void has_all_the_rights_of_the_member()
        {
            _sut.Scope = Setup.AScope;
            _sut.HasRight(SetupUserRights.Right1).Should().BeTrue();
            _sut.HasRight(SetupUserRights.Right2).Should().BeTrue();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }

        public class RightsTests
        {
            [Fact]
            public void global_rights_are_verified_before_local()
            {
                var scope = new ScopeId(Guid.NewGuid());
                var sut =
                    GetSut(new[]
                        {new ScopedRights(ScopeId.Global, new short[] {1}), new ScopedRights(scope, new short[] {2}),});
                sut.Scope = scope;
                sut.HasRight(1).Should().BeTrue();                
                sut.HasRight(2).Should().BeTrue();                
            }

            [Fact]
            public void when_current_scope_is_global_local_is_ignored()
            {
                var sut =
                    GetSut(new[] { new ScopedRights(ScopeId.Global, new short[] { 1 }), new ScopedRights(Setup.AScope, new short[] { 2 }), });
                sut.Scope = ScopeId.Global;
                sut.HasRight(2).Should().BeFalse();
                sut.HasRight(1).Should().BeTrue();
            }

            [Fact]
            public void available_rights_dont_exist_for_current_scope()
            {
                var scope = new ScopeId(Guid.NewGuid());
                var sut =
                    GetSut(new[] { new ScopedRights(ScopeId.Global, new short[] { 1 }), new ScopedRights(scope, new short[] { 2 }), });
                sut.Scope=new ScopeId(Guid.NewGuid());
                sut.HasRight(4, 2).Should().BeFalse();
            }

            public static MemberSessionPrincipal GetSut(IEnumerable<ScopedRights> rights)
            {
                var data = new SessionStorageData()
                {
                    Id = Setup.AnId,
                    MemberInfo = new MemberSessionData()
                    {
                        MemberId = Setup.AnId,
                        MemberName = "name",
                        Rights = rights
                    }
                };
                return new MemberSessionPrincipal(data);
            }
        }
    }

}