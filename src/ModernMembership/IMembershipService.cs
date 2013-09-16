using System;

namespace ModernMembership
{
    public interface IMembershipService
    {
       MembershipCount GetStatistics();
    }
}