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
        //private Guid _id;
        //private LoginName _loginId;
        //private PasswordHash _password;
        //private Email _email;
        //private string _displayName;

        LocalMemberMemento _memento= new LocalMemberMemento();

        public LocalMember(Guid id,LoginName loginId, PasswordHash password, Email email)
        {
            loginId.MustNotBeNull();
            password.MustNotBeNull();
            email.MustNotBeNull();
            _memento.Id = id;
            _memento.LoginId = loginId;
            _memento.Password = password;
            _memento.Email = email;
            _memento.Status=MemberStatus.NeedsActivation;
            _memento.RegisteredOn = DateTime.UtcNow;           
        }

        /// <summary>
        /// Registration date (GMT)
        /// </summary>
        public DateTime RegisteredOn
        {
            get { return _memento.RegisteredOn; }
        }

        /// <summary>
        /// Acts as a cache for a profile name. Convenience only
        /// </summary>
        public string DisplayName
        {
            get { return _memento.DisplayName; }
            set
            {
                _memento.DisplayName = value;                
            }
        }

        public LoginName LoginId
        {
            get { return _memento.LoginId; }
          
        }

        public PasswordHash Password
        {
            get { return _memento.Password; }
           
        }

        public Email Email
        {
            get { return _memento.Email; }
        }

        public void ChangeEmail(Email email)
        {
            email.MustNotBeNull();
            _memento.Email = email;
            _events.Add(new MemberEmailChanged(Id,_memento.Email.Value));
        }

        public void ChangePassword(PasswordHash hash)
        {
            hash.MustNotBeNull();
            _memento.Password = hash;
            _events.Add(new MemberPasswordChanged(Id));
        }

        public MemberStatus Status
        {
            get { return _memento.Status; }
            set { _memento.Status = value; }
        }

        public Guid Id
        {
            get { return _memento.Id; }
        }

        List<MemberEvent> _events=new List<MemberEvent>();
     //   private MemberStatus _status;

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
            //var state = new LocalMemberMemento()
            //    {
            //        DisplayName = DisplayName,
            //        Email = Email,
            //        Id=Id,
            //        LoginId = LoginId,
            //        Password = Password,
            //        RegisteredOn = RegisteredOn,
            //        Status = Status
            //    };
            //return state;
            return _memento;
        }

        protected LocalMember()
        {
            
        }

        public static LocalMember RestoreFrom(LocalMemberMemento state)
        {
            state.MustNotBeNull();
            var member = new LocalMember();
            member._memento = state;
            //member._id = state.Id;
            //member._email=state.Email;
            //member._password = state.Password;
            //member._displayName = state.DisplayName;
            //member.RegisteredOn = state.RegisteredOn;
            //member._loginId=state.LoginId;
            //member._status = state.Status;
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