#if SqlFu
using ModernMembership;
using ModernMembership.SqlFu;
using System;


namespace Tests.Repositories.SqlFu
{
    public class ExternalMembershipRepo:ExternalMembership,IDisposable
    {
        public ExternalMembershipRepo()
        {
            using (var db = SqlFuConfig.GetDb())
            {
                SqlFuMembershipStorage.Init(db);
            }
            
        }
        protected override IExternalMembersRepository GetRepository()
        {
            return new ExternalMembersRepository(SqlFuConfig.GetDb);
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