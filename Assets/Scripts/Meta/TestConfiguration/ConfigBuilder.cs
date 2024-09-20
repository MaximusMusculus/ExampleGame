using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Configs.Conditions;
using Meta.Models;

namespace Meta.TestConfiguration
{
    public class MetaConfigBuilder
    {
        private MetaConfig _config;

        public MetaConfigBuilder()
        {
            _config = new MetaConfig();
        }

        public MetaConfigBuilder NewConfig()
        {
            _config = new MetaConfig();
            return this;
        }

        public MetaConfigBuilder SetConfigVersion(int version)
        {
            _config.ConfigVersion = version;
            return this;
        }

        public MetaConfigBuilder AddItemConfig(Id id, int startCount, int limit = int.MaxValue)
        {
            _config.InventoryItems.Add(new ItemConfig() {Item = id, DefaultCount = startCount, MaxCount = limit});
            return this;
        }
        
        public MetaConfigBuilder AddUnitConfig(UnitConfig unit)
        {
            _config.Units.Add(unit);
            return this;
        }

        public MetaConfigBuilder AddActionConfig(MetaActionConfig metaAction)
        {
            _config.Actions.Add(metaAction);
            return this;
        }

        public MetaConfig Build()
        {
            var metaConfig = _config;
            _config = null;
            return metaConfig;
        }
    }

    public class UnitConfigBuilder
    {
        private UnitConfig _unit;
        private UnitProgressionDto _progression;

        public UnitConfigBuilder NewUnit(Id unitType)
        {
            _unit = new UnitConfig {UnitType = unitType};
            _progression = _unit.GetDefaultProgression();
            return this;
        }

        public UnitConfigBuilder SetCanUpgrade()
        {
            _unit.IsCanUpgrade = true;
            return this;
        }

        public UnitConfigBuilder Progression(int melee, int range, int health)
        {
            _progression.MeleeAttackLevel = melee;
            _progression.RangedAttackLevel = range;
            _progression.HealthLevel = health;
            return this;
        }

        public UnitConfig Build()
        {
            _unit.Progression = _progression;
            var result = _unit;
            _unit = null;
            return result;
        }
    }

    
    public class ActionCollectionConfigBuilder
    {
        private ActionCollectionConfig _config;
        
        public ActionCollectionConfigBuilder NewAction()
        {
            _config = new ActionCollectionConfig();
            return this;
        }

        public ActionCollectionConfigBuilder UnitAdd(UnitConfig unit, int count)
        {
            _config.Untis.Add(new UnitActionConfig {Action = TypeAction.UnitAdd, TypeUnit = unit.UnitType, Progression = unit.Progression, Count = count});
            return this;
        }

        public ActionCollectionConfigBuilder UnitSpend(UnitConfig unit, int count)
        {
            _config.Untis.Add(new UnitActionConfig {Action = TypeAction.UnitSpend, TypeUnit = unit.UnitType, Progression = unit.Progression, Count = count});
            return this;
        }

        public ActionCollectionConfigBuilder InventoryItemAdd(Id itemId, int count)
        {
            _config.Items.Add(new ItemActionConfig {Action = TypeAction.InventoryItemAdd, TypeItem = itemId, Count = count});
            return this;
        }

        public ActionCollectionConfigBuilder InventoryItemSpend(Id itemId, int count)
        {
            _config.Items.Add(new ItemActionConfig {Action = TypeAction.InventoryItemSpend, TypeItem = itemId, Count = count});
            return this;
        }

        public ActionCollectionConfigBuilder InventoryItemExpandLimit(Id itemId, int count)
        {
            _config.Items.Add(new ItemActionConfig {Action = TypeAction.InventoryItemExpandLimit, TypeItem = itemId, Count = count});
            return this;
        }

        public ActionCollectionConfig Build()
        {
            var result = _config;
            _config = null;
            return result;
        }
    }
    
    public enum TypeCollection
    {
        And,
        Or,
    }
    public class ConditionsConfigBuilder
    {
        private ConditionCollectionConfig _config;
        private readonly Dictionary<TypeCollection, TypeCondition> _map = new Dictionary<TypeCollection, TypeCondition>
        {
            {TypeCollection.And, TypeCondition.AndCollection},
            {TypeCollection.Or, TypeCondition.OrCollection},
        };


        public ConditionsConfigBuilder NewCollection(TypeCollection type)
        {
            _config = new ConditionCollectionConfig {TypeCollection = _map[type]};
            return this;
        }

        public ConditionsConfigBuilder ItemCountCondition(Id itemId, TypeCompare compareType, int value)
        {
            _config.CheckItems.Add(new ItemConditionConfig
            {
                TypeCondition = TypeCondition.InventoryItemsCount,
                TypeItem = itemId, 
                CompareType = compareType,
                Value = value
            });
            return this;
        }
        
        public ConditionsConfigBuilder ItemLimitCondition(Id itemId, TypeCompare compareType, int value)
        {
            _config.CheckItems.Add(new ItemConditionConfig
            {
                TypeCondition = TypeCondition.InventoryItemsLimit,
                TypeItem = itemId, 
                CompareType = compareType,
                Value = value
            });
            return this;
        }
        
        public ConditionsConfigBuilder AddCollection(ConditionCollectionConfig config)
        {
            _config.Collection.Add(config);
            return this;
        }
        public ConditionCollectionConfig Build()
        {
            var result = _config;
            _config = null;
            return result;
        }
    }

    public class MetaActionConfigBuilder
    {
        private MetaActionConfig _config;
        
        public MetaActionConfigBuilder NewAction()
        {
            _config = new MetaActionConfig();
            return this;
        }
        
        public MetaActionConfigBuilder SetRequire(ConditionCollectionConfig config)
        {
            _config.Require = config;
            return this;
        }
        
        public MetaActionConfigBuilder SetAdd(ActionCollectionConfig config)
        {
            _config.Add = config;
            return this;
        }
        
        public MetaActionConfigBuilder SetSpend(ActionCollectionConfig config)
        {
            _config.Spend = config;
            return this;
        }
        
        
        public MetaActionConfig Build()
        {
            var result = _config;
            _config = null;
            return result;
        }
    }

}