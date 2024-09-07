using Newtonsoft.Json;
using NUnit.Framework;


namespace Application.Editor
{
    public class TestSerializeDto
    {
        private JsonSerializerSettings _settings;
        
        [OneTimeSetUp]
        public void SetupSettings()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
            _settings.Converters.Add(new IdJsonConverter());
        }
        


        

    }
}