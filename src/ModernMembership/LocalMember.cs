using System.Collections.Generic;
using CavemanTools;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using System;
using DomainBus.Concepts;
using ModernMembership.Events;

namespace ModernMembership
{
    public class LocalMember:IGenerateEvents,ISupportMemento<LocalMemberMemento>
    {
        private Guid _id;
        private LoginName _loginId;
        private PasswordHash _password;
        private Email _email;
        

        public LocalMember(Guid id,LoginName loginId, PasswordHash password, Email email)
        {
            loginId.MustNotBeNull();
            password.MustNotBeNull();
            email.MustNotBeNull();
            _id = id;
            _loginId = loginId;
            _password = password;
            _email = email;
            _status=MemberStatus.NeedsActivation;
            RegisteredOn = DateTime.UtcNow;           
        }

        /// <summary>
        /// Registration date (GMT)
        /// </summary>
        public DateTime RegisteredOn { get; private set; }

        /// <summary>
        /// Acts as a cache for a profile name. Convenience only
        /// </summary>
        public string DisplayName { get; set; }

        public LoginName LoginId
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

        public LocalMemberMemento GetMemento()
        {
            var state = new LocalMemberMemento()
                {
                    DisplayName = DisplayName,
                    Email = Email,
                    Id = Id,
                    LoginId = LoginId,
                    Password = Password,
                    RegisteredOn = RegisteredOn,
                    Status = Status
                };
            return state;            
        }

     
        public static LocalMember RestoreFrom(LocalMemberMemento state)
        {
            state.MustNotBeNull();
            var member = new LocalMember(state.Id,state.LoginId,state.Password,state.Email);
            member.DisplayName = state.DisplayName;
            member.RegisteredOn = state.RegisteredOn;
            member._status = state.Status;
            return member;
        }
    }

    public class LocalMemberMemento
    {
        public Guid Id;

        public LoginName LoginId;

        public PasswordHash Password;

        public Email Email;
        public MemberStatus Status;
        public string DisplayName;
        public DateTime RegisteredOn;        
    }
}