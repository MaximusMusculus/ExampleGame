using System;
using System.Linq;
using Meta.Configs;
using Meta.Models;
using Meta.Tests.Editor.Controllers;
using NUnit.Framework;


namespace Meta.Tests.Editor
{
    [TestFixture]
    public class TestMetaController
    {
        private MetaDto _data;
        private MetaConfig _config;
        private MetaModel _model;
        
        [SetUp]
        public void SetUp()
        {
            _data = new MetaDto();
            _config = new TestMetaConfigProvider().GetConfig();
            _model = new MetaModel(_config, _data);
        }

        [Test]
        public void TestCheckRequireAction()
        {
            var action = _config.Actions.First();
            Assert.IsFalse(_model.CheckRequire(action));
        }
        
        
        [Test]
        public void TestUseAction()
        {
            var action = _config.Actions.First();
            Assert.Throws<InvalidOperationException>(() => _model.RunAction(action));
        }
    }
}