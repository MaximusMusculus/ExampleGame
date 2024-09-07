using System.Collections.Generic;
using NUnit.Framework;


namespace Application.Editor
{
    public class TestIdProvider
    {
        [Test]
        public void TestGetNextId()
        {
            var provider = new IdProvider(new IdProviderDto());
            var dict = new Dictionary<Id, int>();
            for (int i = 0; i < 10; i++)
            {
                dict.Add(provider.GetNext(), i);
            }

            Assert.AreEqual(10, dict.Count);
        }

        [Test]
        public void TestLoadData()
        {
            var data = new IdProviderDto {nextId = 10};
            var provider = new IdProvider(data);
            Assert.AreEqual(10, provider.GetNext().Value);
        }
    }
}