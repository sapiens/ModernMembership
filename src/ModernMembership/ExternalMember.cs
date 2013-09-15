using System;
using System.Collections.Generic;
using DomainBus.Concepts;
using ModernMembership.Events;

namespace ModernMembership
{
    public class ExternalMember:IGenerateEvents
    {
        private readonly Guid _id;
        private readonly ExternalMemberId _externalId;

        public ExternalMember(Guid id, ExternalMemberId externalId, ScopeId scope, string displayName = null, MemberStatus status = ModernMembership.MemberStatus.Active)
        {
           DisplayName = displayName;
            externalId.MustNotBeNull();
            scope.MustNotBeNull();
            _id = id;
            _externalId = externalId;
            _status = status;
            Scope = scope;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public ExternalMemberId ExternalId
        {
            get { return _externalId; }
        }

        public string DisplayName { get; set; }

        public MemberStatus Status
        {
            get { return _status; }
            
            set
            {
                _status = value;
                _events.Add(new MemberStatusChanged(Id,_status));
            }
        }

        public ScopeId Scope { get; private set; }

        List<IEvent> _events =new List<IEvent>();
        private MemberStatus _status;

        public IEnumerable<IEvent> GetGeneratedEvents()
        {
            return _events;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}