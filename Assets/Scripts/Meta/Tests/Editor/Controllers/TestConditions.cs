﻿using System.Collections.Generic;
using Meta.Configs;
using Meta.Configs.Conditions;
using Meta.Controllers;
using Meta.Controllers.Conditions;
using Meta.Controllers.Imp;
using Meta.Models;
using Meta.TestConfiguration;
using NUnit.Framework;

namespace Meta.Tests.Editor.Controllers
{
    [TestFixture]
    public class TestConditions
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly ConditionsConfigBuilder _conditionsBuilder = new ConditionsConfigBuilder();

        private MetaConfig _metaConfig;
        private MetaDto _metaDto;

        private IInventoryController _inventoryController;
        private IConditionProcessor _conditionProcessor;
        
        [SetUp]
        public void SetUp()
        {
            _metaDto = new MetaDto();
            _metaBuilder.NewConfig()
                .AddItemConfig(MapTestId.Scrup.Id(), 100, 100)
                .AddItemConfig(MapTestId.Recruts.Id(), 100, 100);
            
            _metaConfig = _metaBuilder.Build();
            _inventoryController = new InventoryController(_metaConfig.InventoryItems, _metaDto.Items);
            _conditionProcessor = new ConditionProcessor(_inventoryController, new UnitsController(new List<UnitConfig>(), new List<UnitDto>()));
        }
        
        [Test]
        public void TestConditions_InventoryDefault()
        {
            var condition = _conditionsBuilder.NewCollection(TypeCollection.And)
                .ItemCountCondition(MapTestId.Scrup.Id(), TypeCompare.Equal, 100)
                .ItemCountCondition(MapTestId.Recruts.Id(), TypeCompare.Equal, 100)
                .Build();

            var result = _conditionProcessor.Check(condition);
            
            Assert.IsTrue(result);
        }
        
        [Test]
        public void ConditionCoolectionOr()
        {
            var condition1 = _conditionsBuilder.NewCollection(TypeCollection.And)
                .ItemCountCondition(MapTestId.Scrup.Id(), TypeCompare.Equal, 100)
                .ItemCountCondition(MapTestId.Recruts.Id(), TypeCompare.Equal, 100)
                .Build();
            
            var condition2 = _conditionsBuilder.NewCollection(TypeCollection.Or)
                .ItemCountCondition(MapTestId.Scrup.Id(), TypeCompare.Equal, 100)
                .ItemCountCondition(MapTestId.Recruts.Id(), TypeCompare.Equal, 100)
                .Build();

            var condOr = _conditionsBuilder.NewCollection(TypeCollection.Or)
                .AddCollection(condition1)
                .AddCollection(condition2)
                .Build();
            
            var result = _conditionProcessor.Check(condOr);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void TestConditions_InventoryItemsLimit()
        {
            var condition = _conditionsBuilder.NewCollection(TypeCollection.And)
                .ItemLimitCondition(MapTestId.Scrup.Id(), TypeCompare.Less, 101)
                .ItemLimitCondition(MapTestId.Recruts.Id(), TypeCompare.Equal, 100)
                .Build();

            var result = _conditionProcessor.Check(condition);
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestConditions_InventoryItemsLimitFalse()
        {
            var condition = _conditionsBuilder.NewCollection(TypeCollection.And)
                .ItemLimitCondition(MapTestId.Scrup.Id(), TypeCompare.Less, 150)
                .ItemLimitCondition(MapTestId.Recruts.Id(), TypeCompare.Less, 90)
                .Build();

            var result = _conditionProcessor.Check(condition);
            Assert.IsFalse(result);
        }
        
        
        [Test]
        public void TestAndConditions_InventoryNotEqual()
        {
            var condition = _conditionsBuilder.NewCollection(TypeCollection.And)
                .ItemCountCondition(MapTestId.Scrup.Id(), TypeCompare.NotEqual, 100)
                .ItemCountCondition(MapTestId.Recruts.Id(), TypeCompare.Equal, 100)
                .Build();

            var result = _conditionProcessor.Check(condition);
            
            Assert.IsFalse(result);
        }

        [Test]
        public void TestOrConditions_InventoryNotEqual()
        {
            var condition = _conditionsBuilder.NewCollection(TypeCollection.Or)
                .ItemCountCondition(MapTestId.Scrup.Id(), TypeCompare.NotEqual, 100)
                .ItemCountCondition(MapTestId.Recruts.Id(), TypeCompare.Equal, 100)
                .Build();

            var result = _conditionProcessor.Check(condition);
            
            Assert.IsTrue(result);
        }
    


    }
}