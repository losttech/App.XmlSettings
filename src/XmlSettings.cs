namespace LostTech.App
{
    using JetBrains.Annotations;
    using PCLStorage;

    public static class XmlSettings
    {
        public static Settings Create([NotNull] IFolder folder, IFreezerFactory freezerFactory = null)
        {
            freezerFactory = freezerFactory ?? ClonableFreezerFactory.Instance;
            var serializerFactory = new XmlSerializerFactory();
            return new Settings(folder, freezerFactory, serializerFactory, serializerFactory);
        }
    }
}
