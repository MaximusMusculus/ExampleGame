using NUnit.Framework;
using System;
using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Controllers;
using Meta.Models;

[TestFixture]
public class InventoryControllerTests
{
    private IInventoryController _inventoryController;
    private Id _itemId1;
    private Id _itemId2;
    private ItemConfig _item2;
    
    
    [SetUp]
    public void SetUp()
    {
        var item1 = new ItemConfig {Item = 1, DefaultCount = 10, MaxCount = 100};
        var item2 = new ItemConfig {Item = 2, DefaultCount = 5, MaxCount = 50};

        var items = new List<ItemDto>();
        var configs = new List<ItemConfig> {item1, item2};
        _inventoryController = new InventoryController(configs, items);

        _itemId1 = item1.Item;
        _itemId2 = item2.Item;
        _item2 = item2;
    }

    [Test]
    public void Add_IncreasesCountWithinLimit()
    {
        _inventoryController.Add(_itemId1, 20);
        Assert.AreEqual(30, _inventoryController.GetCount(_itemId1));
    }

    [Test]
    public void Add_ExceedsLimit_SetsToLimit()
    {
        _inventoryController.Add(_itemId1, 200);
        Assert.AreEqual(100, _inventoryController.GetCount(_itemId1));
    }

    [Test]
    public void Spend_DecreasesCount()
    {
        _inventoryController.Spend(_itemId1, 5);
        Assert.AreEqual(5, _inventoryController.GetCount(_itemId1));
    }

    [Test]
    public void Spend_NotEnoughItems_ThrowsException()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => _inventoryController.Spend(_itemId1, 15));
        Assert.That(ex.Message, Is.EqualTo("Not enough items:" + _itemId1));
    }

    [Test]
    public void GetCount_ReturnsCurrentCount()
    {
        Assert.AreEqual(10, _inventoryController.GetCount(_itemId1));
    }

    [Test]
    public void GetLimit_ReturnsCurrentLimit()
    {
        Assert.AreEqual(100, _inventoryController.GetLimit(_itemId1));
    }

    [Test]
    public void ExpandLimit_IncreasesLimit()
    {
        _inventoryController.ExpandLimit(_itemId1, 50);
        Assert.AreEqual(150, _inventoryController.GetLimit(_itemId1));
    }

    [Test]
    public void ExpandLimit_HandlesOverflow_ThrowsOverflowException()
    {
        _inventoryController.ExpandLimit(_itemId2, int.MaxValue - _item2.MaxCount);
        Assert.Throws<OverflowException>(() => _inventoryController.ExpandLimit(_itemId2, 1));
    }

}