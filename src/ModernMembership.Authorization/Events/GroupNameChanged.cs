using System;
using DomainBus.Concepts;

namespace ModernMembership.Authorization.Events
{
    public class GroupNameChanged:AbstractEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public GroupNameChanged(Guid id,string name)
        {
            Id = id;
            Name = name;
        }
    }
}