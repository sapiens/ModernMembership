using System.Collections.Generic;
using CavemanTools;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using System;
using DomainBus.Concepts;
using ModernMembership.Events;

namespace ModernMembership
{
    public class LocalMember : IGenerateEvents, ISupportMemento<LocalMember.Memento>
    {
        private Guid _id;
        private LoginName _loginId;
        private PasswordHash _password;
        private Email _email;
        public const MemberStatus DefaultStatus=MemberStatus.NeedsActivation;
        public LocalMember(Memento state)
            : this(state.Id, state.LoginId, state.Password, state.Email, state.Scope)
        {
            state.MustNotBeNull();
            DisplayName = state.DisplayName;
            RegisteredOn = state.RegisteredOn;
            _status = state.Status;
        }

        public LocalMember(Guid id, LoginName loginId, PasswordHash password, Email email, ScopeId scopeId)
        {
            loginId.MustNotBeNull();
            password.MustNotBeNull();
            email.MustNotBeNull();
            scopeId.MustNotBeNull();
            _id = id;
            _loginId = loginId;
            _password = password;
            _email = email;
            _status = DefaultStatus;
            RegisteredOn = DateTime.UtcNow;
            Scope = scopeId;
        }

        /// <summary>
        /// Registration date (GMT)
        /// </summary>
        public DateTime RegisteredOn { get; private set; }

        /// <summary>
        /// Acts as a cache for a profile name. Convenience only
        /// </summary>
        public string DisplayName { get; set; }

        public LoginName Name
        {
            get { return _loginId; }

        }

        public PasswordHash Password
        {
            get { return _password; }

        }

        public Email Email
        {
            get { return _email; }
        }

        public void ChangeName(LoginName name)
        {
            name.MustNotBeNull();
            _loginId = name;
            _events.Add(new MemberLoginNameChanged(Id,name.Value));
        }


        public void ChangeEmail(Email email)
        {
            email.MustNotBeNull();
            _email = email;
            _events.Add(new MemberEmailChanged(Id,_email.Value));
        }

        public void ChangePassword(PasswordHash hash)
        {
            hash.MustNotBeNull();
            _password = hash;
            _events.Add(new MemberPasswordChanged(Id));
        }

        public MemberStatus Status
        {
            get { return _status; }
            set
            {
                if (value==MemberStatus.Undefined) throw new ArgumentException("Undefined is not an accepted value");
                _status = value;
                _events.Add(new MemberStatusChanged(Id,_status));
            }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public ScopeId Scope { get; private set; }

        List<MemberEvent> _events=new List<MemberEvent>();
        private MemberStatus _status;
        
        public IEnumerable<IEvent> GetGeneratedEvents()
        {
            return _events;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public Memento GetMemento()
        {
            var state = new Memento()
                {
                    DisplayName = DisplayName,
                    Email = Email,
                    Id = Id,
                    LoginId = Name,
                    Password = Password,
                    RegisteredOn = RegisteredOn,
                    Status = Status,
                    Scope = Scope
                };
            return state;            
        }
        
        public class Memento
        {
            public Guid Id;

            public LoginName LoginId;

            public PasswordHash Password;

            public Email Email;
            public MemberStatus Status;
            public string DisplayName;
            public DateTime RegisteredOn;
            public ScopeId Scope;
        }
       
    }

    
}