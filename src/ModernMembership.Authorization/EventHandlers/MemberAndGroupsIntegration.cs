using System;
using DomainBus.Concepts;
using ModernMembership.Events;
using System.Linq;

namespace ModernMembership.Authorization.EventHandlers
{
    public class MemberAndGroupsIntegration:ISubscribeTo<LocalMemberDeleted>,ISubscribeTo<ExternalMemberDeleted>
    {
        private readonly IUserGroupsRepository _groupsRepo;
        private readonly IDispatchMessages _bus;

        public MemberAndGroupsIntegration(IUserGroupsRepository groupsRepo,IDispatchMessages bus)
        {
           groupsRepo.MustNotBeNull();
            _groupsRepo = groupsRepo;
            _bus = bus;
        }

        public void Handle(LocalMemberDeleted evnt)
        {
            RemoveUserFromGroups(evnt.MemberId);
        }

        public void Handle(ExternalMemberDeleted evnt)
        {
            RemoveUserFromGroups(evnt.MemberId);
        }

        void RemoveUserFromGroups(Guid userId)
        {
            var grps = _groupsRepo.GetGroupsForUser(userId).ToArray();
            foreach (var grp in grps)
            {
                grp.RemoveUsers(userId);
            }
            _groupsRepo.Save(grps);
            var events = grps.SelectMany(g => g.GetGeneratedEvents()).ToArray();
            _bus.Publish(events);
        }
    }
}