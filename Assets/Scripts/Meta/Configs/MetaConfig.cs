using System.Collections.Generic;
using AppRen;

namespace Meta.Configs
{
    public class MetaConfig
    {
        public int ConfigVersion;
        //данные, что показывет интерфейс игрока (например счетчик ресурсов) тоже формируются и модифицируются в конфиге...
        public List<ItemConfig> InventoryItems = new List<ItemConfig>();
        public List<UnitConfig> Units = new List<UnitConfig>();
    }
    
    
    public class ItemConfig
    {
        public Id Item;
        public int DefaultCount; //??
        public int MaxCount;
        //nameKey
        //Icon?

        public string TestGuid;
    }
      
    
    
}