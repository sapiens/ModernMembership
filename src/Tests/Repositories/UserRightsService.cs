using System;
using System.Linq;
using ModernMembership;
using ModernMembership.Authorization;
using Xunit;
using FluentAssertions;

namespace Tests.Repositories
{
    public class MemoryUserRightsService:UserRightsService
    {
        private UserGroupsMemoryRepository _service;

        protected override IUserRightsService GetService()
        {
            _service= new UserGroupsMemoryRepository();
            return _service;
        }

        protected override void SetupRights()
        {
            var group1 = Setup.SomeRightsGroup("gp1");
            var group2 = Setup.SomeRightsGroup("gp2");
            group1.Rights.Add(SetupUserRights.Right1);
            group2.Rights.Add(SetupUserRights.Right2);
            _service.Add(group1);
            _service.Add(group2);

            var userGroup1 = new UserGroup(group1.Id, new[] {Setup.AFixedId});
            var userGroup2 = new UserGroup(group2.Id, new[] {Setup.AFixedId});

            _service.Add(userGroup1);
            _service.Add(userGroup2);
        }
    }
    
    public abstract class UserRightsService
    {
        protected IUserRightsService _sut;

        public UserRightsService()
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
            var scoped=_sut.GetRights(Setup.AFixedId);
            scoped.Count().Should().Be(1);
            var rights = scoped.First();
            rights.HasRight(SetupUserRights.Right1).Should().BeTrue();
            rights.HasRight(SetupUserRights.Right2).Should().BeTrue();

        }
    }
}