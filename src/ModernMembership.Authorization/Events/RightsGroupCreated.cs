using System;
using DomainBus.Concepts;

namespace ModernMembership.Authorization.Events
{
    public class RightsGroupCreated:AbstractEvent
    {
        public Guid GroupId { get; set; }

        public RightsGroupCreated()
        {
            
        }

        public RightsGroupCreated(RightsGroup group)
        {
            GroupId = group.Id;
            Name = group.Name;
            Scope = group.Scope;
        }

        public ScopeId Scope { get; set; }

        public GroupName Name { get; set; }
    }
}