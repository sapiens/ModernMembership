using ModernMembership;

namespace Tests.Repositories.Memory
{
    public class MemoryExternalmembership:BaseExternalMembershipTests
    {
        protected override IExternalMembersRepository GetRepository()
        {
            return new ExternalMembersMemoryRepository();
        }
    }
}