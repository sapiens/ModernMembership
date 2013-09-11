using System;
using System.Collections.Generic;
using ModernMembership.Authorization;

namespace ModernMembership.Web
{
    public class MemberSessionData
    {
        public Guid MemberId;
        public string MemberName;
        public IEnumerable<ScopedRights> Rights;
    }
}