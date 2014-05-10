using System;
using System.Collections.Generic;
using CavemanTools;

namespace ModernMembership
{
    public class MembershipService : IMembershipService
    {
        private readonly ILocalMembersRepository _local;
        private readonly IExternalMembersRepository _external;

        public MembershipService(ILocalMembersRepository local,IExternalMembersRepository external)
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

        public string GetDisplayName(Guid id)
        {
            var user = _local.GetMember(id);
            if (user != null)
            {
                return user.DisplayName??user.Name.Value;
            }
            var exuser = _external.GetMember(id);
            if (exuser != null)
            {
                return exuser.DisplayName ?? exuser.ExternalId.ToString();
            }
            return "";
        }
    }
}