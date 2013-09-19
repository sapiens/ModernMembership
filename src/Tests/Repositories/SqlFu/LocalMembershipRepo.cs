#if SqlFu
using ModernMembership;
using ModernMembership.SqlFu;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.Repositories.SqlFu
{
    public class LocalMembershipRepo:LocalMembershipActions,IDisposable
    {
        public LocalMembershipRepo()
        {
            using (var db = SqlFuConfig.GetDb())
            {
                SqlFuMembershipStorage.Init(db);
            }
            
        }
        protected override ILocalMembersRepository GetRepository()
        {
            return new LocalMembersRepository(SqlFuConfig.GetDb);
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