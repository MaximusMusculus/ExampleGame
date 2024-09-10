using System.Collections.Generic;
using Mapster;
using NUnit.Framework;


namespace TesMapping
{
    [TestFixture]
    public class TestMapster
    {
        private TypeAdapterConfig _config;

        [SetUp]
        public void SetUp()
        {
            _config = new TypeAdapterConfig();
            new RegisterMapper().Register(_config);
        }

        [Test]
        public void TestSingleElemMapping()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "TestName",
                Count = 42
            };

            // Act
            var target = source.Adapt<TargetClass>(_config);

            // Assert
            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.Count, target.Count);
        }

        [Test]
        public void TestCollectionMapping()
        {
            var sourceCollection = new SourceCollection
            {
                Collection = new List<SourceClass>
                {
                    new SourceClass {Name = "Item1", Count = 1},
                    new SourceClass {Name = "Item2", Count = 2},
                    new SourceClass {Name = "Item3", Count = 3}
                }
            };

            // Act
            var targetCollection = sourceCollection.Adapt<TargetCollection>(_config);

            // Assert
            Assert.AreEqual(sourceCollection.Collection.Count, targetCollection.Collection.Count);
            for (int i = 0; i < sourceCollection.Collection.Count; i++)
            {
                Assert.AreEqual(sourceCollection.Collection[i].Name, targetCollection.Collection[i].Name);
                Assert.AreEqual(sourceCollection.Collection[i].Count, targetCollection.Collection[i].Count);
            }
        }

    }

    public class RegisterMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SourceClass, TargetClass>().RequireDestinationMemberSource(true);
            config.NewConfig<SourceCollection, TargetCollection>().RequireDestinationMemberSource(true);
        }
    }

    public class SourceCollection
    {
        public List<SourceClass> Collection;
    }

    public class TargetCollection
    {
        public List<TargetClass> Collection;
    }

    public class SourceClass
    {
        public string Name;
        public int Count;
    }

    public class TargetClass
    {
        public string Name;
        public int Count;
    }

}
