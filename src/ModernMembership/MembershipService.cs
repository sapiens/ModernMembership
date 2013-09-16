using System.Collections.Generic;
using CavemanTools;

namespace ModernMembership
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipStats _local;
        private readonly IMembershipStats _external;

        public MembershipService(IMembershipStats local,IMembershipStats external)
        {
            _local = local;
            _external = external;
        }

        public MembershipCount GetStatistics()
        {
            var local = _local.GetStats();
            var ext = _external.GetStats();

            var all = new Dictionary<MemberStatus, int>();

            foreach (var status in EnumUtils.AsValues<MemberStatus>())
            {
                if (status == MemberStatus.Undefined) continue;
                
                all[status] = 0;
                if (local.ContainsKey(status)) all[status] += local[status];
                if (ext.ContainsKey(status)) all[status] += ext[status];
            }
            return new MembershipCount(local.Count, ext.Count, all);
        }
    }
}