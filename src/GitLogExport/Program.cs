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
            await new JsonWriter().Execute(new GitLog(@"d:\Projects\DotNetTest\nodatime").GetCommits(), @"d:\Projects\DotNetTest\nodatime\gitLog.json");
        }
    }
}
