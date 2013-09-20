using ModernMembership.Authorization;

namespace Tests.Repositories.Memory
{
    public class MemoryRightsGroups:BaseRightsGroupsTests
    {
        protected override IRightsGroupsRepository GetRepo()
        {
            return new UserGroupsMemoryRepository();
        }
    }
}