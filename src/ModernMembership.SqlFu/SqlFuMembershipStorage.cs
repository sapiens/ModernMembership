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
                PerformAutomaticMigrations();
        }

        public static void Destroy(DbConnection cnx)
        {
           InitMembersStorage.Destroy(cnx);
           InitGroupStorage.Destroy(cnx);
        }
    }

    [Migration("1.0.0",SchemaName = Schema,Priority = 90)]
    public class InitGroupStorage:AbstractMigrationTask
    {
        internal const string Schema = "SqlFu.Authorization";
        /// <summary>
        /// Task is executed automatically in a transaction
        /// </summary>
        /// <param name="db"/>
        public override void Execute(DbConnection db)
        {
            db.CreateTable<RightsGroupData>();
            db.CreateTable<UserGroupData>();
        }

        public static void Destroy(DbConnection cnx)
        {
            cnx.Drop<UserGroupData>();
            cnx.Drop<RightsGroupData>();
            DatabaseMigration.ConfigureFor(cnx)
                .SearchCurrentAssembly()
                .BuildAutomaticMigrator()
                .Untrack(Schema);
        }
    }


    public class InitSessionStorage:AbstractMigrationTask
    {
        /// <summary>
        /// Task is executed automatically in a transaction
        /// </summary>
        /// <param name="db"/>
        public override void Execute(DbConnection db)
        {
            throw new System.NotImplementedException();
        }
    }


    [Migration("1.0.0",SchemaName = Schema,Priority = 100)]
    public class InitMembersStorage:AbstractMigrationTask
    {
        internal const string Schema = "SqlFu.Members";

        /// <summary>
        /// Task is executed automatically in a transaction
        /// </summary>
        /// <param name="db"/>
        public override void Execute(DbConnection db)
        {
            db.CreateTable<LocalMemberData>();
            db.CreateTable<ExternalMemberData>();
        }

        public static void Destroy(DbConnection cnx)
        {
            cnx.Drop<LocalMemberData>();
            cnx.Drop<ExternalMemberData>();
            DatabaseMigration.ConfigureFor(cnx)
                .SearchCurrentAssembly()
                .BuildAutomaticMigrator()
                .Untrack(Schema);
        }
    }
}