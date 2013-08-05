using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.EventHandlers
{
    public class WhenRightGroupIsDeleted
    {
        private Stopwatch _t = new Stopwatch();

        public WhenRightGroupIsDeleted()
        {

        }

        [Fact]
        public void usergroup_is_deleted()
        {
            throw new NotImplementedException();
            
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}