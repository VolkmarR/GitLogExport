using GitLogExport.Extractor;
using GitLogExport.Writer;
using System;
using System.Threading.Tasks;

namespace GitLogExport
{
    class Program
    {
        async static Task Main(string[] args)
        {
            //var repo = @"d:\Projects\DotNetTest\nodatime";
            //var output = @"d:\Projects\DotNetTest\nodatime\gitLog.json";

            var repo = @"..\..\..\..\..\.";
            var output = @"gitLog.json";
            var filter = new Filter {  };

            await new JsonWriter().Execute(new GitLog(repo).GetCommits(filter), output);
        }
    }
}
