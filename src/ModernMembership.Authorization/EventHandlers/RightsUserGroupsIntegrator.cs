using DomainBus.Concepts;
using ModernMembership.Authorization.Events;

namespace ModernMembership.Authorization.EventHandlers
{
    public class RightsUserGroupsIntegrator:ISubscribeTo<RightsGroupCreated>,ISubscribeTo<RightsGroupDeleted>
    {
        private readonly IUserGroupsRepository _repository;

        public RightsUserGroupsIntegrator(IUserGroupsRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RightsGroupCreated evnt)
        {
            var group = new UserGroup(evnt.GroupId);
            _repository.Add(group);
        }

        public void Handle(RightsGroupDeleted evnt)
        {
            _repository.Delete(evnt.GroupId);
        }
    }
}