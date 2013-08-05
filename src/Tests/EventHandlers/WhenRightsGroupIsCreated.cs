using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.EventHandlers
{
    public class WhenRightsGroupIsCreated
    {
        private Stopwatch _t = new Stopwatch();

        public WhenRightsGroupIsCreated()
        {

        }

        [Fact]
        public void usergroup_is_created()
        {
            throw new NotImplementedException();
            
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}