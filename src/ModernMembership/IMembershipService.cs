using System;

namespace ModernMembership
{
    public interface IMembershipService
    {
        /// <summary>
        /// Deletes local members who didn't activate their account in the specified period
        /// </summary>
        /// <param name="interval"></param>
        void PurgeUnactivatedMembers(TimeSpan interval);

        MembershipCount GetStatistics();
    }
}