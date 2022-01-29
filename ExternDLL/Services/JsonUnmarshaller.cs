using System.Text.Json;

namespace JTLVersandImport.Services
{
    public class JsonUnmarshaller
    {
        public static T Unmarshall<T>(string JsonString) where T : class
        {

            return JsonSerializer.Deserialize<T>(JsonString);
        }
    }
}
