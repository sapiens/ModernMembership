using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
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
            var grp = Setup.AnEmptyUserGroup();
            _sut.Add(grp);
            var g2 = _sut.GetUserGroup(grp.Id);
            g2.Should().NotBeNull();
        }

        [Fact]
        public void get_nonexisting_group_returns_null()
        {
            _sut.GetUserGroup(Guid.NewGuid()).Should().BeNull();
        }

        [Fact]
        public void delete()
        {
            var grp = Setup.AnEmptyUserGroup();
            _sut.Add(grp);
            _sut.Delete(grp.Id);
            _sut.GetUserGroup(grp.Id).Should().BeNull();
        }

        [Fact]
        public void save_group()
        {
            var grp = Setup.AnEmptyUserGroup();
            _sut.Add(grp);
            var u2 = _sut.GetUserGroup(grp.Id);
            u2.AddUsers(Guid.NewGuid());
            _sut.Save(u2);

            var u3 = _sut.GetUserGroup(grp.Id);
            u3.Users.Count().Should().Be(1);
        }

        [Fact]
        public void get_groups_for_user()
        {
            var grp1 = Setup.AnEmptyUserGroup();
            var user = Guid.NewGuid();
            grp1.AddUsers(user);
            _sut.Add(grp1);

            var grp2 = Setup.AnEmptyUserGroup();
            grp2.AddUsers(user);
            _sut.Add(grp2);

            var all = _sut.GetGroupsForUser(user);
            all.Count().Should().Be(2);
            all.Any(d => d.Id == grp1.Id).Should().BeTrue();
            all.Any(d => d.Id == grp2.Id).Should().BeTrue();
        }


        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}