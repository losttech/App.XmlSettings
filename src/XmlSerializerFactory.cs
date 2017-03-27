namespace LostTech.App
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class XmlSerializerFactory : ISerializerFactory, IDeserializerFactory
    {
        public XmlReaderSettings ReaderSettings { get; } = new XmlReaderSettings();
        public XmlWriterSettings WriterSettings { get; } = new XmlWriterSettings {
            Indent = true,
        };

        public Func<Stream, Task<T>> MakeDeserializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            var readerSettings = this.ReaderSettings.Clone();
            return stream => {
                var xmlReader = XmlReader.Create(stream, readerSettings);
                return Task.FromResult((T)serializer.Deserialize(xmlReader));
            };
        }

        public Func<Stream, T, Task> MakeSerializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            var writerSettings = this.WriterSettings.Clone();
            return (stream, value) => {
                var xmlWriter = XmlWriter.Create(stream, writerSettings);
                serializer.Serialize(stream, value);
                return Task.CompletedTask;
            };
        }
    }
}
