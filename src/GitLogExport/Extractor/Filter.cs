using System;
using System.Collections.Generic;
using System.Text;

namespace GitLogExport.Extractor
{
    public class Filter
    {
        public string CommitSha { get; set; }
        public string CommitsAfterSha { get; set; }
        public string CommitsToSha { get; set; }
    }
}
