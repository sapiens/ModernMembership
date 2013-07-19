using System;

namespace ModernMembership.Authorization.Events
{
    public class UsersRemovedFromGroup:UsersAddedToGroup
    {
        public UsersRemovedFromGroup(Guid grpId, params Guid[] users) : base(grpId, users)
        {
        }
    }
}