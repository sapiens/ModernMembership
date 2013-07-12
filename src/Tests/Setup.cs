using CavemanTools.Model.ValueObjects;
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

        public static Email GetSomeEmail
        {
            get
            {
                return new Email("test@example.com");
            }
        }
    }
}