namespace LostTech.App
{
    using System.IO;

    public static class XmlSettings
    {
        public static Settings Create(DirectoryInfo folder, IFreezerFactory? freezerFactory = null)
        {
            freezerFactory ??= ClonableFreezerFactory.Instance;
            var serializerFactory = new XmlSerializerFactory();
            return new Settings(folder, freezerFactory, serializerFactory, serializerFactory);
        }
    }
}
