using SampleSegmenter.Interfaces;
using SampleSegmenter.Options;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleSegmenter.Services
{
    public class OptionsService : IOptionsService
    {
        public static async Task SaveOptionsAsync(AllOptions options, string fileName)
        {
            using FileStream createStream = File.Create(fileName);

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            await JsonSerializer.SerializeAsync(createStream, options, jsonOptions);
            await createStream.DisposeAsync();

        }

        public static AllOptions LoadOptionsAsync(string fileName)
        {
            using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return JsonSerializer.DeserializeAsync<AllOptions>(fileStream).Result;
        }
    }
}
