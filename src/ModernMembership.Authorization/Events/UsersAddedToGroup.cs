using System;
using System.Collections.Generic;
using DomainBus.Concepts;

namespace ModernMembership.Authorization.Events
{
    public class UsersAddedToGroup:AbstractEvent
    {
        public Guid GroupId { get; set; }
        public List<Guid> Users { get; private set; }

        public UsersAddedToGroup(Guid grpId, params Guid[] users)
        {
            GroupId = grpId;
            Users = new List<Guid>();
            Users.AddRange(users);
        }
    }
}