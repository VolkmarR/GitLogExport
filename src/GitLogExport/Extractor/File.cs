using System;
using System.Collections.Generic;
using System.Text;

namespace GitLogExport.Extractor
{
    public class File
    {
        public string Path { get; set; }
        public int LinesAdded { get; set; }
        public int LinesDeleted { get; set; }
        public FileStatus Status { get; set; }
    }
}
