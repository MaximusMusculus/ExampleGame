using System.Collections.Generic;

namespace Meta.Configs
{
    public class MetaConfig
    {
        public int ConfigVersion;
        //данные, что показывет интерфейс игрока (например счетчик ресурсов) тоже формируются и модифицируются в конфиге...
        public List<ItemConfig> InventoryItems = new List<ItemConfig>();
        public List<UnitConfig> Units = new List<UnitConfig>();
        public List<MetaActionConfig> Actions = new List<MetaActionConfig>();
    }
}