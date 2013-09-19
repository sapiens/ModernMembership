using System.Data.Common;
using SqlFu;

namespace ModernMembership.SqlFu
{
    public class SqlFuMembershipStorage
    {
        public static void Init(DbConnection cnx)
        {
            cnx.CreateTable<Models.LocalMemberData>();
        }

        public static void Destroy(DbConnection cnx)
        {
            cnx.Drop<Models.LocalMemberData>();
        }
    }
}