using System;
using DomainBus.Concepts;

namespace ModernMembership.Authorization.Events
{
    public class RightsGroupDeleted:AbstractEvent
    {
        public Guid GroupId { get; set; }

        public RightsGroupDeleted(Guid groupId)
        {
            GroupId = groupId;
        }
    }


}