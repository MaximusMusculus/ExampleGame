using System;
using System.Collections.Generic;
using AppRen;
using Meta.Models;


namespace Meta.Controllers
{
    public interface IInventoryController
    {
        void Add(Id item, int count);
        void Spend(Id item, int count);
        int GetCount(Id item);
        int GetLimit(Id item);
    }

    public class InventoryController : IInventoryController
    {
        private readonly List<ItemDto> _items;
        private readonly Dictionary<Id, ItemDto> _itemsHash;

        public InventoryController(List<ItemDto> items)
        {
            _items = items;
            _itemsHash = new Dictionary<Id, ItemDto>(_items.Count);
            foreach (var item in items)
            {
                _itemsHash[item.Id] = item;
            }
        }

        public void Add(Id item, int count)
        {
            var elem = _itemsHash[item];
            elem.Count = Math.Clamp(elem.Count + count, 0, elem.Limit);
        }

        public void Spend(Id item, int count)
        {
            var elem = _itemsHash[item];
            if (elem.Count < count)
            {
                throw new InvalidOperationException("Not enough items:" + item);
            }
            elem.Count -= count;
        }

        public int GetCount(Id item)
        {
            return _itemsHash[item].Count;
        }

        public int GetLimit(Id item)
        {
            return _itemsHash[item].Limit;
        }
    }
}