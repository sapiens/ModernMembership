using System;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using ModernMembership;
using ModernMembership.Authorization;
using Ploeh.AutoFixture;

namespace Tests
{
    public class Setup
    {
        static Fixture _fixture=new Fixture();
        public static Fixture GetAutoFixture()
        {
            return _fixture;
        }

        public static Guid AnId=Guid.NewGuid();

        public static ScopeId AScope=new ScopeId(Guid.NewGuid());

        public static Email SomeEmail
        {
            get
            {
                return new Email("test@example.com");
            }
        }

        public static SetupPassword APassword
        {
            get
            {
                return new SetupPassword();
            }
        }

        public static RightsGroup CreateRightsGroup()
        {
            return new RightsGroup(Guid.NewGuid(),new GroupName("test"));
        }

        public static RightsGroup CreateExistingRightsGroup()
        {
            return new RightsGroup(new RightsGroup.Memento()
                {
                    Id=Guid.NewGuid(),Name = new GroupName("test")
                });
        }
    }

    public class SetupPassword
    {
        public string Value = "pwd";
        public PasswordHash Hash
        {
            get
            {
                var str = new CavemanHashStrategy();
                return new PasswordHash(str.Hash(Value, "salt"), "salt");
            }
        }
    }
}