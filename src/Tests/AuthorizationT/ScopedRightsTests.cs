using ModernMembership;
using ModernMembership.Authorization;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit.Extensions;

namespace Tests.AuthorizationT
{
    public class GlobalScopedRightsTests
    {
        private Stopwatch _t = new Stopwatch();
        private ScopedRights _sut;

        public GlobalScopedRightsTests()
        {
            _sut = new ScopedRights(ScopeId.Global, new int[] { 2, 5});
        }

        [Theory]
        [InlineData((int)2,(int)5)]
        [InlineData((int)2,(int)7)]
        [InlineData((int)9,(int)5)]
        public void existing_rights_pass_check(int right1,int right2)
        {
            _sut.HasRight(right1, right2).Should().BeTrue();
        }

        [Fact]
        public void when_rights_dont_exist_check_fails()
        {
            _sut.HasRight(4).Should().BeFalse();
        }

        [Fact] 
        public void when_has_admin_right_checks_always_pass()
        {
            var sut = Setup.UserRights.GlobalAdminRights;
            sut.HasRight(23).Should().BeTrue();
        }


        [Fact]
        public void when_has_scoped_admin_check_fails()
        {
            var sut = new ScopedRights(ScopeId.Global, new int[] {ScopedRights.ScopedAdmin});
            sut.HasRight(23).Should().BeFalse();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }

    public class LocalScopedRightsTest
    {
        public LocalScopedRightsTest()
        {
            
        }

        [Fact]
        public void when_has_admin_right_checks_always_pass()
        {
            var sut = Setup.UserRights.ScopedAdminRights;
            sut.HasRight(23).Should().BeTrue();
        }


        [Fact]
        public void when_has_global_admin_check_fails()
        {
            var sut = new ScopedRights(Setup.ARandomScope, new int[] { ScopedRights.GlobalAdmin });
            
            sut.HasRight(23).Should().BeFalse();
        }
    }
}