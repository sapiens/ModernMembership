using System.Linq;
using ModernMembership.Authorization;
using ModernMembership.Authorization.Events;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.AuthorizationT
{
    public class UserGroupsT
    {
        private Stopwatch _t = new Stopwatch();
        private UserGroup _ug;

        public UserGroupsT()
        {
            _ug=new UserGroup(Guid.NewGuid());
        }

        [Fact]
        public void user_group_requires_rightsgroup_id()
        {
            var id = Guid.NewGuid();
            var ug = new UserGroup(id);
            ug.Id.Should().Be(id);
        }

        [Fact]
        public void newly_created_usergroup_has_no_users()
        {
            _ug.Users.Should().BeEmpty();
        }

        [Fact]
        public void create_group_with_inital_users()
        {
            var ug = new UserGroup(Guid.NewGuid(), new[] {Guid.NewGuid()});
            ug.Users.Count().Should().Be(1);
        }


        [Fact]
        public void adding_users_to_group_should_generate_corresponding_event()
        {
            var uid = Guid.NewGuid();
            _ug.AddUsers(uid);
            var evnt = _ug.GetGeneratedEvents().First().Cast<UsersAddedToGroup>();
            evnt.GroupId.Should().Be(_ug.Id);
            evnt.Users.Should().Contain(uid);
            _ug.Users.Last().Should().Be(uid);
        }

        [Fact]
        public void adding_existing_user_is_ignored()
        {
            var uid = Guid.NewGuid();
            _ug.AddUsers(uid);
            _ug.ClearEvents();
            _ug.AddUsers(uid);
            _ug.Users.Count().Should().Be(1);
            _ug.GetGeneratedEvents().Should().BeEmpty();
        }

        [Fact]
        public void all_added_users_are_contained_by_event()
        {
            var id = Guid.NewGuid();
            var sut = new UserGroup(Guid.NewGuid(), new[] {id});
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            sut.AddUsers(id,id2,id3);
            var evnt = sut.GetGeneratedEvents().First().Cast<UsersAddedToGroup>();
            evnt.Users.Count.Should().Be(2);
            evnt.Users.First().Should().Be(id2);
            evnt.Users.Last().Should().Be(id3);
        }

        [Fact]
        public void remove_user_generates_corresponding_event()
        {
            var id = Guid.NewGuid();
            var sut = new UserGroup(Guid.NewGuid(), new []{id});
            sut.RemoveUsers(id);
            sut.Users.Should().BeEmpty();
            var evnt = sut.GetGeneratedEvents().First().Cast<UsersRemovedFromGroup>();
            evnt.GroupId.Should().Be(sut.Id);
            evnt.Users.Count.Should().Be(1);
            evnt.Users.First().Should().Be(id);

        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}