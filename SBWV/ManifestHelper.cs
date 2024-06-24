using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SBWV
{
    public static class ManifestHelper
    {
        private static readonly Dictionary<string, string> _manifest;

        static ManifestHelper()
        {
            try
            {
                var manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dist", "manifest.json");
                if (File.Exists(manifestPath))
                {
                    var manifestJson = File.ReadAllText(manifestPath);
                    Console.WriteLine($"Manifest JSON content: {manifestJson}"); // Выводим содержимое для отладки
                    
                    _manifest = JsonConvert.DeserializeObject<Dictionary<string, string>>(manifestJson);
                }
                else
                {
                    _manifest = new Dictionary<string, string>();
                    // Логирование или выброс исключения, если файл не найден
                    throw new FileNotFoundException($"Manifest file not found: {manifestPath}");
                }
            }
            catch (JsonReaderException jsonEx)
            {
                // Логирование ошибки парсинга JSON
                Console.WriteLine($"Error parsing JSON: {jsonEx.Message}");
                throw new ApplicationException("Error parsing manifest.json", jsonEx);
            }
            catch (Exception ex)
            {
                // Логирование исключения
                Console.WriteLine($"Failed to initialize ManifestHelper: {ex.Message}");
                // Повторное выбрасывание исключения
                throw;
            }
        }

        public static string GetAssetPath(string key)
        {
            return _manifest.TryGetValue(key, out var value) ? value : key;
        }
    }
}
