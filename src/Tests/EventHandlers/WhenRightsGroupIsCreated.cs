using System.Collections.Generic;
using CavemanTools.Model;
using FakeItEasy;
using ModernMembership;
using ModernMembership.Authorization;
using ModernMembership.Authorization.EventHandlers;
using ModernMembership.Authorization.Events;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.EventHandlers
{
    public class WhenRightsGroupIsCreated
    {

        class FakeRepo:IUserGroupsRepository
        {
            List<UserGroup> grp=new List<UserGroup>();

            public void Add(UserGroup @group)
            {
                grp.Add(@group);
            }

            

            public UserGroup GetUserGroup(Guid groupId)
            {
                return grp.Find(d => d.Id == groupId);
            }

            public void Save(params UserGroup[] @group)
            {
                throw new NotImplementedException();
            }

            public void Delete(Guid grpId)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="skip"></param>
            /// <param name="take"></param>
            /// <param name="scope">Use ScopeId.None for global scope</param>
            /// <returns></returns>
            public PagedResult<UserGroup> GetPagedUserGroups(int skip, int take, ScopeId scope)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<UserGroup> GetUserGroups(IEnumerable<Guid> ids)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<UserGroup> GetGroupsForUser(Guid userId)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<RightsGroup> GetRightsGroupsForUser(Guid userId)
            {
                throw new NotImplementedException();
            }
        }

        private Stopwatch _t = new Stopwatch();
        private IUserGroupsRepository _repo;
        private RightsUserGroupsIntegrator _sut;

        public WhenRightsGroupIsCreated()
        {
            _repo = new FakeRepo();
            _sut = new RightsUserGroupsIntegrator(_repo);
        }

        [Fact]
        public void an_empty_usergroup_is_created()
        {
            var grp = Setup.SomeRightsGroup();
            _sut.Handle(new RightsGroupCreated(grp));
            var usrGrp = _repo.GetUserGroup(grp.Id);
            usrGrp.Users.Should().BeEmpty();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}