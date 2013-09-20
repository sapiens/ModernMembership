using ModernMembership.Authorization;

namespace Tests.Repositories.Memory
{
    public class MemoryUserRightsService:BaseUserRightsServiceTests
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
}