using FakeItEasy;
using ModernMembership.Authorization;
using ModernMembership.Authorization.EventHandlers;
using ModernMembership.Authorization.Events;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.EventHandlers
{
    public class WhenRightGroupIsDeleted
    {
        private Stopwatch _t = new Stopwatch();
        private RightsUserGroupsIntegrator _sut;
        private Guid _id;
        private IUserGroupsRepository _usrGrpRepo;

        public WhenRightGroupIsDeleted()
        {
            _usrGrpRepo = A.Fake<IUserGroupsRepository>();
            _sut = new RightsUserGroupsIntegrator(_usrGrpRepo);
            _id = Guid.NewGuid();
            
        }

        [Fact]
        public void usergroup_is_deleted()
        {
            _sut.Handle(new RightsGroupDeleted(_id));            
            A.CallTo(()=>_usrGrpRepo.Delete(_id)).MustHaveHappened(Repeated.Exactly.Once);
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}