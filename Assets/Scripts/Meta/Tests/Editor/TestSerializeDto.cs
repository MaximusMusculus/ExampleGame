using Meta.Configs;
using Meta.TestConfiguration;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Meta.Tests.Editor
{
    public class TestSerialize
    {
        private JsonSerializerSettings _settings;
        
        [OneTimeSetUp]
        public void SetupSettings()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };
            _settings.Converters.Add(new JsonConverterDtoId());
        }

        [Test]
        public void ShowSerializedJsonConfig()
        {
            var config = new MetaConfigDevelopProvider().GetConfig();
            //new MetaConfigProviderTestBig().GetConfig();   don't do this ^_^
                
            config.ConfigVersion = 777;

            var json = JsonConvert.SerializeObject(config, _settings);
            Debug.Log(json);
            var configDeserialized = JsonConvert.DeserializeObject<MetaConfig>(json, _settings);
            Assert.AreEqual(config.ConfigVersion, configDeserialized.ConfigVersion);
        }
        
        
    }
}