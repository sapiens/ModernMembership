using System;
using System.Data.Common;
using ModernMembership.SqlFu.Models;
using SqlFu;
using SqlFu.Expressions;
using Xunit;

namespace Tests.Repositories.SqlFu
{
    public class SqlFuConfig
    {
        public const string Connection = @"Data Source=.;Initial Catalog=tempdb;Integrated Security=True";

        static SqlFuConfig()
        {
            SqlFuDao.OnCommand = cmd =>
                {
                    Console.WriteLine(cmd.FormatCommand());
                };
            SqlFuDao.OnException = (cmd, ex) =>
                {
                   Console.WriteLine("{0}\n{1}",cmd.FormatCommand(),ex);
                };
        }

        public static DbConnection GetDb()
        {
            return new SqlFuConnection(Connection, DbEngine.SqlServer);
        }      
    }
}