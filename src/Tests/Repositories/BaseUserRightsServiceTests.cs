using System;
using System.Linq;
using ModernMembership;
using ModernMembership.Authorization;
using Xunit;
using FluentAssertions;

namespace Tests.Repositories
{
    public abstract class BaseUserRightsServiceTests
    {
        protected IUserRightsService _sut;

        public BaseUserRightsServiceTests()
        {
            _sut = GetService();
        }

        
        protected abstract IUserRightsService GetService();

        /// <summary>
        /// Two user groups must be configured, containing SetupUserRights constants
        /// </summary>
        protected abstract void SetupRights();

        [Fact]
        public void get_scoped_rights()
        {
            SetupRights();
            var scoped=_sut.GetRights(Setup.AFixedId);
            scoped.Count().Should().Be(1);
            var rights = scoped.First();
            rights.HasRight(SetupUserRights.Right1).Should().BeTrue();
            rights.HasRight(SetupUserRights.Right2).Should().BeTrue();

        }
    }
}