#if SqlFu
using System;
using ModernMembership.Authorization;
using ModernMembership.SqlFu;

namespace Tests.Repositories.SqlFu
{
    public class UserRightsRepo:BaseUserRightsServiceTests,IDisposable
    {
        private UserRightsRepository _service;

        public UserRightsRepo()
        {
            using (var db = SqlFuConfig.GetDb())
            {
                SqlFuMembershipStorage.Init(db);
            }
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

        protected override IUserRightsService GetService()
        {
           _service=new UserRightsRepository(SqlFuConfig.GetDb);
            return _service;
        }

        /// <summary>
        /// Two user groups must be configured, containing SetupUserRights constants
        /// </summary>
        protected override void SetupRights()
        {
            var group1 = Setup.SomeRightsGroup("gp1");
            var group2 = Setup.SomeRightsGroup("gp2");
            group1.Rights.Add(SetupUserRights.Right1);
            group2.Rights.Add(SetupUserRights.Right2);
            _service.Add(group1);
            _service.Add(group2);

            var userGroup1 = new UserGroup(group1.Id, new[] { Setup.AFixedId });
            var userGroup2 = new UserGroup(group2.Id, new[] { Setup.AFixedId });

            _service.Add(userGroup1);
            _service.Add(userGroup2);
        }
    }
}
#endif