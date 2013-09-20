#if SqlFu
using System;
using ModernMembership.Authorization;
using ModernMembership.SqlFu;

namespace Tests.Repositories.SqlFu
{
    public class RightsGroupsRepo:RightsGroups,IDisposable
    {
        public RightsGroupsRepo()
        {
            using (var db = SqlFuConfig.GetDb())
            {
                SqlFuMembershipStorage.Init(db);
            }
        }
        protected override IRightsGroupsRepository GetRepo()
        {
            return new UserRightsRepository(SqlFuConfig.GetDb);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            using (var db = SqlFuConfig.GetDb())
            {
                SqlFuMembershipStorage.Destroy(db);
            }
        }
    }
}
#endif