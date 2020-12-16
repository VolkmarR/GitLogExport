using System;
using System.Collections.Generic;
using System.Text;

namespace GitLogExport.Extractor
{
    public class Commit
    {
        public string Hash { get; set; }
        public Author Author { get; set; }
        public Author Committer { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public List<File> Files { get; set; } = new List<File>();
    }
}
