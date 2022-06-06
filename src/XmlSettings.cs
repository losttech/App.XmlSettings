namespace LostTech.App
{
    using System.IO;

    /// <summary>
    /// Helper methods to create XML-based <see cref="Settings"/>
    /// </summary>
    public static class XmlSettings
    {
        /// <summary>
        /// Create a <see cref="Settings"/> object that saves in XML files inside the specified <paramref name="folder"/>.
        /// </summary>
        /// <param name="folder">A folder, that contains XML files with specific <see cref="SettingsSet{T, TFrozen}">SettingsSets</see></param>
        /// <param name="freezerFactory">Optional freezer factory. If none specified, <see cref="ClonableFreezerFactory"/> is used.</param>
        public static Settings Create(DirectoryInfo folder, IFreezerFactory? freezerFactory = null)
        {
            freezerFactory ??= ClonableFreezerFactory.Instance;
            var serializerFactory = new XmlSerializerFactory();
            return new Settings(folder, freezerFactory, serializerFactory, serializerFactory);
        }
    }
}
