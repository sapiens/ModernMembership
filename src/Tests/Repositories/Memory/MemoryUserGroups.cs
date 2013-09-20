using ModernMembership.Authorization;

namespace Tests.Repositories.Memory
{
    public class MemoryUserGroups:BaseUserGroupsTests
    {
        protected override IUserGroupsRepository GetRepo()
        {
            return new UserGroupsMemoryRepository();
        }
    }
}