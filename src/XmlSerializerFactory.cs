namespace LostTech.App
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// <see cref="XmlSerializer"/>-based implementation for <see cref="ISerializerFactory"/>
    /// and <see cref="IDeserializerFactory"/> required for <see cref="Settings"/>.
    /// </summary>
    /// <remarks>The simplest way to use this is via <see cref="XmlSettings.Create(System.IO.DirectoryInfo, IFreezerFactory?)"/></remarks>
    public sealed class XmlSerializerFactory : ISerializerFactory, IDeserializerFactory
    {
        /// <summary>
        /// XML reading settings
        /// </summary>
        public XmlReaderSettings ReaderSettings { get; } = new XmlReaderSettings {};
        /// <summary>
        /// XML writing settings.
        /// </summary>
        public XmlWriterSettings WriterSettings { get; } = new XmlWriterSettings {
            Indent = true,
            Async = true,
            Encoding = Encoding.UTF8,
        };

        /// <inheritdoc/>
        public Func<Stream, Task<T>> MakeDeserializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            var readerSettings = this.ReaderSettings.Clone();
            return stream => {
                using var xmlReader = XmlReader.Create(stream, readerSettings);
                try {
                    return Task.FromResult((T)serializer.Deserialize(xmlReader));
                } catch (InvalidOperationException formatException) {
                    throw new FormatException(formatException.Message, formatException);
                }
            };
        }

        /// <inheritdoc/>
        public Func<Stream, T, Task> MakeSerializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            var writerSettings = this.WriterSettings.Clone();
            return async (stream, value) => {
                using var xmlWriter = XmlWriter.Create(stream, writerSettings);
                serializer.Serialize(xmlWriter, value);
                if (writerSettings.Async)
                    await xmlWriter.FlushAsync().ConfigureAwait(false);
                else
                    xmlWriter.Flush();
            };
        }
    }
}
