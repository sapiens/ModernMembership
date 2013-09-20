using ModernMembership.Web;

namespace Tests.Repositories.Memory
{
    public class MemoryMemberSessions:BaseMemberSessionsTests
    {
        protected override IMemberSessionStorage GetStorage()
        {
            return new SessionMemoryStorage();
        }
    }
}