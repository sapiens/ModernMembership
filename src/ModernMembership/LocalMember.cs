using System.Collections.Generic;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using System;
using DomainBus.Concepts;
using ModernMembership.Events;

namespace ModernMembership
{
    public class LocalMember:IGenerateEvents
    {
        private readonly Guid _id;
        private LoginName _name;
        private PasswordHash _password;
        private Email _email;
        private string _displayName;

        public LocalMember(Guid id,LoginName name, PasswordHash password, Email email)
        {
            name.MustNotBeNull();
            password.MustNotBeNull();
            email.MustNotBeNull();
            _id = id;
            _name = name;
            _password = password;
            _email = email;
            Status=MemberStatus.NeedsActivation;
            RegisteredOn = DateTime.UtcNow;
            _displayName = name.Value;
        }
        /// <summary>
        /// Registration date (GMT)
        /// </summary>
        public DateTime RegisteredOn { get; private set; }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                value.MustNotBeEmpty();
                _displayName = value;
                _events.Add(new MemberNameChanged(Id){Name = value});
            }
        }

        public LoginName Name
        {
            get { return _name; }
            //set
            //{
            //    value.MustNotBeNull();
            //    _name = value;
            //}
        }

        public PasswordHash Password
        {
            get { return _password; }
            //set
            //{
            //    value.MustNotBeNull();
            //    _password = value;
            //}
        }

        public Email Email
        {
            get { return _email; }
        }

        public void ChangeEmail(Email email)
        {
            email.MustNotBeNull();
            _email = email;
            _events.Add(new MemberEmailChanged(Id,email.Value));
        }

        public MemberStatus Status { get; set; }

        public Guid Id
        {
            get { return _id; }
        }

        List<MemberEvent> _events=new List<MemberEvent>();

        public IEnumerable<IEvent> GetGeneratedEvents()
        {
            return _events;
        }
    }
}