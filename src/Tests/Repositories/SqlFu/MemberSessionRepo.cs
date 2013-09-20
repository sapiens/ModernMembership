#if SqlFu
using System;
using ModernMembership.SqlFu;
using ModernMembership.Web;

namespace Tests.Repositories.SqlFu
{
    public class MemberSessionRepo:BaseMemberSessionsTests,IDisposable
    {
        public MemberSessionRepo()
        {
            using (var db = SqlFuConfig.GetDb())
            {
                SqlFuMembershipStorage.Init(db);
            }
        }
        protected override IMemberSessionStorage GetStorage()
        {
            return new SessionStorage(SqlFuConfig.GetDb);
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