using System.Reflection;

namespace Compression.App
{
    internal static class PluginLoader
    {
        public static T[] Load<T>()
        {
            var libFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var plugins = new List<T>();

            foreach (var file in libFiles)
            {
                var assembly = Assembly.LoadFrom(file);
                var pluginTypes = assembly.GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                foreach (var pluginType in pluginTypes)
                {
                    var plugin = (T)Activator.CreateInstance(pluginType)!;
                    plugins.Add(plugin);
                }
            }

            return plugins.ToArray();
        }
    }
}
