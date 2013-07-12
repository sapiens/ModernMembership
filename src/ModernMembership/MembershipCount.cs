using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernMembership
{
    public class MembershipCount
    {
        private readonly IEnumerable<KeyValuePair<MemberStatus, int>> _perStatus;

        public MembershipCount(int local,int external,IEnumerable<KeyValuePair<MemberStatus,int>> perStatus)
        {
            _perStatus.MustNotBeEmpty();
            _perStatus.MustComplyWith(s=> Enum.GetValues(typeof(MemberStatus)).Length-1==s.Count(),"Need a number for each status except Undefined");
            _perStatus = perStatus;
            LocalMembers = local;
            ExternalMembers = external;
        }

        public int LocalMembers { get; private set; }
        public int ExternalMembers { get; private set; }
        
        
        public int TotalMembers
        {
            get { return LocalMembers + ExternalMembers; }
        }

        public int this[MemberStatus status]
        {
            get
            {
                if (status==MemberStatus.Undefined) throw new ArgumentException("Undefined is not a valid status");
                return _perStatus.First(s => s.Key == status).Value;
            }
        }

    }
}