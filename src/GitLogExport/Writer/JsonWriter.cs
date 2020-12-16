using GitLogExport.Extractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GitLogExport.Writer
{
    public class JsonWriter
    {
        public async Task Execute(IEnumerable<Commit> commits, string fileName)
        {
            using FileStream createStream = System.IO.File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, commits, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } } );
        }
    }
}
