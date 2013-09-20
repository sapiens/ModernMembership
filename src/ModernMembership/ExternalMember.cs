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
        private readonly DateTime _registeredOn;
        
        public const MemberStatus DefaultStatus=MemberStatus.Active;

        public ExternalMember(Guid id, ExternalMemberId externalId, ScopeId scope,string displayName = null, MemberStatus status = DefaultStatus):this(id,externalId,scope,DateTime.UtcNow,displayName,status)
        {
            
        }

        public ExternalMember(Guid id, ExternalMemberId externalId, ScopeId scope,DateTime registeredOn, string displayName = null, MemberStatus status = DefaultStatus)
        {
           DisplayName = displayName;
            externalId.MustNotBeNull();
            scope.MustNotBeNull();
            _id = id;
            _externalId = externalId;
            _registeredOn = registeredOn;
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
                _events.Add(new MemberStatusChanged(Id,_status,false));
            }
        }

        public ScopeId Scope { get; private set; }

        public DateTime RegisteredOn
        {
            get { return _registeredOn; }
        }

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