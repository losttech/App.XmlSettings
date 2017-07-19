namespace LostTech.App
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;
    public class SerializerTests
    {
        [Fact]
        public async Task SerializerWorks()
        {
            var factory = new XmlSerializerFactory();
            var serializer = factory.MakeSerializer<string>();
            var memoryStream = new MemoryStream();
            await serializer(memoryStream, "42");
            Assert.NotEqual(0, memoryStream.Length);
        }
    }
}
