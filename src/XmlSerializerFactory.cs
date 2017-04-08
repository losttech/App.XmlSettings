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
            Async = true,
        };

        public Func<Stream, Task<T>> MakeDeserializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            var readerSettings = this.ReaderSettings.Clone();
            return stream => {
                using (var xmlReader = XmlReader.Create(stream, readerSettings)) {
                    try {
                        return Task.FromResult((T) serializer.Deserialize(xmlReader));
                    }
                    catch (InvalidOperationException formatException) {
                        throw new FormatException(formatException.Message, formatException);
                    }
                }
            };
        }

        public Func<Stream, T, Task> MakeSerializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            var writerSettings = this.WriterSettings.Clone();
            return async (stream, value) => {
                using (var xmlWriter = XmlWriter.Create(stream, writerSettings)) {
                    serializer.Serialize(stream, value);
                    if (writerSettings.Async)
                        await xmlWriter.FlushAsync().ConfigureAwait(false);
                    else
                        xmlWriter.Flush();
                }
            };
        }
    }
}
