using System;
using System.Collections.Generic;
using DomainBus.Concepts;
using ModernMembership.Authorization.Events;

namespace ModernMembership.Authorization
{
    public class UserGroup:IGenerateEvents
    {
        public UserGroup(Guid grpId,IEnumerable<Guid> users=null)
        {
            Id = grpId;
            if (users != null)
            {
                _users.AddRange(users);
            }
        }
        public Guid Id { get; private set; }

        List<Guid> _users=new List<Guid>();

        public IEnumerable<Guid> Users
        {
            get { return _users; }
        }
        private List<IEvent> _events = new List<IEvent>();

        public void AddUsers(params Guid[] ids)
        {
            if (ids.Length==0) return;
            var evnt = new UsersAddedToGroup(Id);
            foreach (var id in ids)
            {
                if (_users.AddIfNotPresent(id))
                {
                    evnt.Users.Add(id);
                }
            }
            if (evnt.Users.Count > 0)
            {
                _events.Add(evnt);
            }
        }

        public IEnumerable<IEvent> GetGeneratedEvents()
        {
            return _events;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public void RemoveUsers(params Guid[] users)
        {
            if (users.Length==0) return;
            var evnt = new UsersRemovedFromGroup(Id);
            foreach (var id in users)
            {
               if (_users.Remove(id))
               {
                   evnt.Users.Add(id);
               }
            }

            if (evnt.Users.Count > 0)
            {
                _events.Add(evnt);
            }
        }
    }
}