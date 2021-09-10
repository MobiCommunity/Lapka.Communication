using System;
using System.IO;

namespace Lapka.Communication.Core.ValueObjects
{
    public class PhotoFile
    {
        public Guid Id { get; }
        public string Name { get; }
        public Stream Content { get; }
        public string ContentType { get; }

        public PhotoFile(Guid id, string name, Stream content, string contentType)
        {
            Id = id;
            Name = name;
            Content = content;
            ContentType = contentType;
        }
    }
}