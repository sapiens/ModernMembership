using System;
using System.Collections.Generic;
using CavemanTools;
using DomainBus.Concepts;
using ModernMembership.Authorization.Events;

namespace ModernMembership.Authorization
{
    public class RightsGroup:IGenerateEvents,ISupportMemento<RightsGroup.Memento>
    {
        private Guid _id;
        private GroupName _name;
        private List<short> _rights;

        public RightsGroup(Memento init):this(init.Id,init.Name,init.Scope)
        {
            if (init.Rights != null)
            {
                _rights.AddRange(init.Rights);
            }
        }

        public RightsGroup(Guid id, GroupName name,ScopeId scope=null)
        {
            id.MustComplyWith(i=>i!=Guid.Empty,"Invalid guid");
            name.MustNotBeNull();
            _id = id;
            _name = name;
            _rights = new List<short>();
            Scope = scope;
            _events.Add(new RightsGroupCreated(this));
        }

        public Guid Id
        {
            get { return _id; }        
        }

        public GroupName Name
        {
            get { return _name; }
            set
            {
                value.MustNotBeNull();
                _name = value;
                _events.Add(new GroupNameChanged(Id,_name.Value));
            }
        }

        public List<short> Rights
        {
            get { return _rights; }
        }

        public ScopeId Scope { get; private set; }
        private List<IEvent> _events = new List<IEvent>();
        public IEnumerable<IEvent> GetGeneratedEvents()
        {
            return _events;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public class Memento
        {
            public IEnumerable<short> Rights;
            public Guid Id;
            public GroupName Name;
            public ScopeId Scope;
        }

        public Memento GetMemento()
        {
            return new Memento()
                {
                    Id=Id,Name = Name,Rights = _rights.ToArray(),Scope = Scope
                };
        }
      }
}