using FluentAssertions;
using ModernMembership.Authorization;
using Xunit;

namespace Tests.AuthorizationT
{
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