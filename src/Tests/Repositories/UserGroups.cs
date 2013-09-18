using System;
using System.Diagnostics;
using ModernMembership.Authorization;
using Xunit;

namespace Tests.Repositories
{
    public class MemoryUserGroups:UserGroups
    {
        protected override IUserGroupsRepository GetRepo()
        {
            return new UserGroupsMemoryRepository();
        }
    }

    public abstract class UserGroups
    {
        private Stopwatch _t = new Stopwatch();
        private IUserGroupsRepository _sut;

        public UserGroups()
        {
            _sut = GetRepo();
        }

        protected abstract IUserGroupsRepository GetRepo();

        [Fact]
        public void add_get_group()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void get_nonexisting_group_returns_null()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void delete()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void save_group()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void get_groups_for_user()
        {
            throw new NotImplementedException();  
        }

        [Fact]
        public void get_rights_group_for_user()
        {
            throw new NotImplementedException();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}