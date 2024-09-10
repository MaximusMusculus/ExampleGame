using System.Collections.Generic;
using Application;

namespace Meta.Configs
{
    public class MetaConfig
    {
        public int ConfigVersion;
        //данные, что показывет интерфейс игрока (например счетчик ресурсов) тоже формируются и модифицируются в конфиге...
        public List<UnitConfig> Units = new List<UnitConfig>();
        public List<ResourceConfig> Resources = new List<ResourceConfig>();
    }

    
    public class ResourceConfig
    {
        //public Id ResourceType;

        public string id;
        //nameKey
        //Icon?
    }
      
    
    
}