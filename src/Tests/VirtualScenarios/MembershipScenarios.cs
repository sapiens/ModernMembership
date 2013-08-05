using FakeItEasy;
using ModernMembership;
using System;
using System.Diagnostics;

namespace Tests.VirtualScenarios
{
    public class MembershipScenarios
    {
        private Stopwatch _t = new Stopwatch();
        private ILocalMembersRepository _localRepo;
        private IExternalMembersRepository _externalRepo;

        public MembershipScenarios()
        {
            _localRepo = A.Fake<ILocalMembersRepository>();
            _externalRepo = A.Fake<IExternalMembersRepository>();
        }

       
        public void recover_password_needs_to_get_member_by_email()
        {
            _localRepo.GetMember(Setup.SomeEmail);
        }

       
        public void delete_multiple_local_members()
        {
            _localRepo.Delete(Guid.NewGuid(),Guid.NewGuid());
        }


        public void delete_multiple_external_members()
        {
            _externalRepo.Delete(Guid.NewGuid(),Guid.NewGuid());
        }

        
        public void can_get_paged_list_of_local_members()
        {
            _localRepo.GetMembers(10, 15, ScopeId.None);//can be scoped
        }
        
        public void can_get_paged_list_of_external_members()
        {
            //scope is not required, an external member is always global
            _externalRepo.GetMembers(10, 15);            
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}