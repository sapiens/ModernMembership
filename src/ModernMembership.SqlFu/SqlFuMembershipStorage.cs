using System.Data.Common;
using ModernMembership.SqlFu.Models;
using SqlFu;
using SqlFu.Migrations;

namespace ModernMembership.SqlFu
{
    public class SqlFuMembershipStorage
    {
        public static void Init(DbConnection cnx)
        {
            DatabaseMigration.ConfigureFor(cnx).
                SearchCurrentAssembly().
                PerformAutomaticMigrations("SqlFu.Membership");
        }

        public static void Destroy(DbConnection cnx)
        {
            cnx.Drop<LocalMemberData>();
            cnx.Drop<ExternalMemberData>();
            DatabaseMigration.ConfigureFor(cnx)
                .SearchCurrentAssembly()
                .BuildAutomaticMigrator()
                .Untrack("SqlFu.Membership");
        }
    }

    [Migration("1.0.0",SchemaName = "SqlFu.Membership")]
    public class InitStorage:AbstractMigrationTask
    {
        /// <summary>
        /// Task is executed automatically in a transaction
        /// </summary>
        /// <param name="db"/>
        public override void Execute(DbConnection db)
        {
            db.CreateTable<LocalMemberData>();
            db.CreateTable<ExternalMemberData>();
        }
    }
}