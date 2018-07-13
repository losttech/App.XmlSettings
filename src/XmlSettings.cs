namespace LostTech.App
{
    using System.IO;
    using JetBrains.Annotations;

    public static class XmlSettings
    {
        public static Settings Create([NotNull] DirectoryInfo folder, IFreezerFactory freezerFactory = null)
        {
            freezerFactory = freezerFactory ?? ClonableFreezerFactory.Instance;
            var serializerFactory = new XmlSerializerFactory();
            return new Settings(folder, freezerFactory, serializerFactory, serializerFactory);
        }
    }
}
