using System;

namespace ModernMembership
{
    public class ExternalMember
    {
        private readonly Guid _id;
        private readonly ExternalMemberId _externalId;

        public ExternalMember(Guid id, ExternalMemberId externalId,string displayName=null)
        {
           DisplayName = displayName;
            externalId.MustNotBeNull();
            _id = id;
            _externalId = externalId;
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
    }
}