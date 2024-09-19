using System;
using AppRen;
using Meta.Configs.Actions;
using Meta.Configs.Conditions;
using Meta.Models;

namespace Meta.Configs.TestConfiguration
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

        public MetaConfigBuilder AddActionConfig(ActionCollectionConfig action)
        {
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

    public class ActionConfigBuilder
    {
        private ActionCollectionConfig _config;

        public ActionConfigBuilder NewAction()
        {
            _config = new ActionCollectionConfig();
            return this;
        }

        public ActionConfigBuilder UnitAdd(UnitConfig unit, int count)
        {
            _config.Actions.Add(new UnitActionConfig {Action = TypeAction.UnitAdd, TypeUnit = unit.UnitType, Progression = unit.Progression, Count = count});
            return this;
        }

        public ActionConfigBuilder UnitSpend(UnitConfig unit, int count)
        {
            _config.Actions.Add(new UnitActionConfig {Action = TypeAction.UnitSpend, TypeUnit = unit.UnitType, Progression = unit.Progression, Count = count});
            return this;
        }

        public ActionConfigBuilder InventoryItemAdd(Id itemId, int count)
        {
            _config.Actions.Add(new ItemActionConfig {Action = TypeAction.InventoryItemAdd, TypeItem = itemId, Count = count});
            return this;
        }

        public ActionConfigBuilder InventoryItemSpend(Id itemId, int count)
        {
            _config.Actions.Add(new ItemActionConfig {Action = TypeAction.InventoryItemSpend, TypeItem = itemId, Count = count});
            return this;
        }

        public ActionConfigBuilder InventoryItemExpandLimit(Id itemId, int count)
        {
            _config.Actions.Add(new ItemActionConfig {Action = TypeAction.InventoryItemExpandLimit, TypeItem = itemId, Count = count});
            return this;
        }

        public ActionCollectionConfig Build()
        {
            var result = _config;
            _config = null;
            return result;
        }
    }


    public class ConditionsConfigBuilder
    {
        private ConditionCollectionConfig _config;
        
        public enum TypeCollection
        {
            And,
            Or,
        }
        public ConditionsConfigBuilder NewCollection(TypeCollection type)
        {
            switch (type)
            {
                case TypeCollection.And:
                    _config = new ConditionCollectionConfig {TypeCollection = TypeCondition.AndCollection};
                    break;
                case TypeCollection.Or:
                    _config = new ConditionCollectionConfig {TypeCollection = TypeCondition.OrCollection};
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return this;
        }
        
        public ConditionsConfigBuilder AddItemCondition(Id itemId, int compareType, int value)
        {
            _config.Conditions.Add(new ItemConditionConfig {TypeItem = itemId, CompareType = compareType, Value = value});
            return this;
        }
        
        public ConditionCollectionConfig Build()
        {
            var result = _config;
            _config = null;
            return result;
        }

    }
}