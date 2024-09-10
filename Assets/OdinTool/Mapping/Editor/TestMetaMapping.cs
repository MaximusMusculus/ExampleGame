﻿using Mapster;
using Meta.ConfigOdin;
using Meta.Configs;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace TesMapping
{
    [TestFixture]
    public class TestMetaMapping
    {
        private TypeAdapterConfig _config;
        
        [SetUp]
        public void SetUp()
        {
            _config = new TypeAdapterConfig();
            new MetaConfigMapper().Register(_config);
        }
        
        [Test]
        public void TestMetaResource()
        {
            // Arrange
            var path = "Assets/OdinTool/MetaConfig/Config/Resources/ResourceHard.asset";
            var source = AssetDatabase.LoadAssetAtPath<ResourceConfigOdin>(path);
            
            // Act
            var target = source.Adapt<ResourceConfig>(_config);
            
            // Assert
            Assert.AreEqual( source.GetGuid(), target.id);
        }

    }


    public static class GetUidExtension
    {
        public static string GetGuid(this ScriptableObject myData)
        {
            var assetPath = AssetDatabase.GetAssetPath(myData);
            return AssetDatabase.AssetPathToGUID(assetPath);
        }
    }

    public class MetaConfigMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ResourceConfigOdin, ResourceConfig>().RequireDestinationMemberSource(true)
                .Map(dest => dest.id, src => src.GetGuid());


            /*config.NewConfig<Meta.ConfigOdin.MetaConfigOdin, Meta.Config.MetaConfig>()
                .Map(dest => dest.Resources, src => src.Resources)
                .Map(dest => dest.Units, src => src.Units)
                .Map(dest => dest.Quests, src => src.Quests);*/
        }
    }
}