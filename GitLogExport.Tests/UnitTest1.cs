using GitLogExport.Extractor;
using System;
using System.Linq;
using Xunit;

namespace GitLogExport.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var extractor = new GitLog(@"d:\Projects\DotNetTest\nodatime");
            var commits = extractor.GetCommits().ToList();

        }
    }
}
