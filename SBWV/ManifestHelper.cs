using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SBWV
{


    public class Icon
    {
        public string Density { get; set; }
        public string Sizes { get; set; }
        public string Src { get; set; }
        public string Type { get; set; }
    }

    public class AppManifest
    {
        public string Name { get; set; }
        public List<Icon> Icons { get; set; }
    }

    public static class ManifestHelper
    {
        private static readonly AppManifest _manifest;

        static ManifestHelper()
        {
            try
            {
                var manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dist", "manifest.json");
                if (File.Exists(manifestPath))
                {
                    var manifestJson = File.ReadAllText(manifestPath);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    _manifest = JsonSerializer.Deserialize<AppManifest>(manifestJson, options);

                    if (_manifest == null)
                    {
                        throw new Exception("Failed to deserialize manifest.json. The resulting object is null.");
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Manifest file not found: {manifestPath}");
                }
            }
            catch (Exception ex)
            {
                // Логирование исключения
                Console.WriteLine($"Failed to initialize ManifestHelper: {ex.Message}");
                // Повторное выбрасывание исключения
                throw;
            }
        }

        public static string GetIconPath(string size)
        {
            if (_manifest?.Icons != null)
            {
                var icon = _manifest.Icons.Find(i => i.Sizes == size);
                return icon?.Src ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
