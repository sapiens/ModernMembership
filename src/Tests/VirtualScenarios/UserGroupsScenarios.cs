using FakeItEasy;
using ModernMembership.Authorization;
using Xunit;
using System;
using System.Diagnostics;

namespace Tests.VirtualScenarios
{
    public class UserGroupsScenarios
    {
        private Stopwatch _t = new Stopwatch();
        private IUserGroupsRepository _repository;

        public UserGroupsScenarios()
        {
            _repository = A.Fake<IUserGroupsRepository>();
        }

      
        public void add_remove_users_to_multiple_groups_require_save_multiple_groups()
        {
            _repository.Save(A.Fake<UserGroup>(),A.Fake<UserGroup>());
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}