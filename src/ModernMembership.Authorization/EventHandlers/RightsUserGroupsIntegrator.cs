using DomainBus.Concepts;
using ModernMembership.Authorization.Events;

namespace ModernMembership.Authorization.EventHandlers
{
    public class RightsUserGroupsIntegrator:ISubscribeTo<RightsGroupCreated>,ISubscribeTo<RightsGroupDeleted>
    {
        public void Handle(RightsGroupCreated evnt)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(RightsGroupDeleted evnt)
        {
            throw new System.NotImplementedException();
        }
    }
}