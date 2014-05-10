using System;

namespace ModernMembership
{
    public interface IMembershipService
    {
       MembershipCount GetStatistics();
       
       /// <summary>
       /// Gets display name if set or the user name if not
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        string GetDisplayName(Guid id);
    }
}