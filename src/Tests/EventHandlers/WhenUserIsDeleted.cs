using System.Collections.Generic;
using DomainBus.Concepts;
using FakeItEasy;
using ModernMembership.Authorization;
using ModernMembership.Authorization.EventHandlers;
using ModernMembership.Events;

using System;
using System.Diagnostics;
using FluentAssertions;
using System.Linq;
using Xunit.Extensions;

namespace Tests.EventHandlers
{
    public class WhenUserIsDeleted
    {
        private Stopwatch _t = new Stopwatch();
        private MemberAndGroupsIntegration _sut;
        private Guid _userId;
        private IUserGroupsRepository _repository;
        private IDispatchMessages _bus;

        public WhenUserIsDeleted()
        {
            _userId = Guid.NewGuid();
            _repository = A.Fake<IUserGroupsRepository>();

            var groups = CreateGroups();
            A.CallTo(() => _repository.GetGroupsForUser(_userId)).Returns(groups);
            _bus = A.Fake<IDispatchMessages>();
            _sut = new MemberAndGroupsIntegration(_repository,_bus);
        }

        IEnumerable<UserGroup> CreateGroups()
        {
            var grp1 = new UserGroup(Guid.NewGuid(), new[] {Guid.NewGuid(), _userId});
            var grp2 = new UserGroup(Guid.NewGuid(), new[] {Guid.NewGuid(), _userId});
            return new[] {grp1,grp2};
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void is_removed_from_groups(bool local)
        {
            if (local)
            {
                _sut.Handle(new LocalMemberDeleted(_userId));
            }
            else
            {
                _sut.Handle(new ExternalMemberDeleted(_userId));
            }
            
            A.CallTo(()=>_repository.Save(A<UserGroup>.Ignored,A<UserGroup>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            
            var grps = _repository.GetGroupsForUser(_userId).ToArray();
            grps[0].Users.Any(d => d == _userId).Should().Be(false);
            grps[1].Users.Any(d => d == _userId).Should().Be(false);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void user_removal_events_are_published(bool local)
        {
            if (local)
            {
                _sut.Handle(new LocalMemberDeleted(_userId));
            }
            else
            {
                _sut.Handle(new ExternalMemberDeleted(_userId));
            }
  
            A.CallTo(()=>_bus.Publish(A<IEvent[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}