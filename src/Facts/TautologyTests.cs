using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Facts
{
    public class TautologyTests
    {
        [Fact]
        public void AlwaysTrue()
        {
            Assert.True(true);
        }
    }
}
