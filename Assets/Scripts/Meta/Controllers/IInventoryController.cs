using System;
using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Models;


namespace Meta.Controllers
{
    public interface IInventoryController
    {
        void Add(Id item, int count);
        void Spend(Id item, int count);
        int GetCount(Id item);
        
        int GetLimit(Id item);
        void ExpandLimit(Id item, int count);
    }

    public class InventoryController : IInventoryController
    {
        private readonly Dictionary<Id, ItemDto> _itemsHash;
        public InventoryController(List<ItemConfig> configs, List<ItemDto> items)
        {
            _itemsHash = new Dictionary<Id, ItemDto>(configs.Count);
            foreach (var item in items)
            {
                _itemsHash[item.Id] = item;
            }

            //добавляем новые айтемы с конфига 
            foreach (var itemConfig in configs)
            {
                if (_itemsHash.ContainsKey(itemConfig.Item))
                {
                    continue;
                }
                var item = new ItemDto
                {
                    Id = itemConfig.Item,
                    Count = itemConfig.DefaultCount,
                    Limit = itemConfig.MaxCount
                };
                items.Add(item);
                _itemsHash.Add(item.Id, item);
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

        public void ExpandLimit(Id item, int count)
        {
            var elem = _itemsHash[item];
            elem.Limit = checked(elem.Limit + count);
        }
    }
}