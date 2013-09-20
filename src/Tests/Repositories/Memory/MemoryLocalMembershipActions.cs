using System;
using ModernMembership;

namespace Tests.Repositories.Memory
{
    public class MemoryLocalMembershipActions:BaseLocalMembershipTests,IDisposable
    {
        protected override ILocalMembersRepository GetRepository()
        {
            return new LocalMembersMemoryRepository();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            (_sut as LocalMembersMemoryRepository).Dispose();
        }
    }
}