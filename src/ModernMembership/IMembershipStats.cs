using System.Collections.Generic;

namespace ModernMembership
{
    public interface IMembershipStats
    {
        IDictionary<MemberStatus, int> GetStats();
    }
}