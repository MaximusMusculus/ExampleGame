using AppRen;
using Meta.Configs.Actions.Imp;
using Meta.Models;

namespace Meta.TestConfiguration
{
    public class ActionConfigFactory
    {
        public UnitActionConfig CreateUnitAddAction(Id unitType,int count, UnitProgressionDto progression = null)
        {
            progression ??= new UnitProgressionDto();
            return new UnitActionConfig(UnitActionConfig.TypeUnitAction.UnitAdd)
            {
                TypeUnit = unitType,
                Progression = progression,
                Count = count
            };
        }
        public UnitActionConfig CreateUnitSpendAction(Id unitType, int count, UnitProgressionDto progression = null)
        {
            progression ??= new UnitProgressionDto();
            return new UnitActionConfig(UnitActionConfig.TypeUnitAction.UnitSpend)
            {
                TypeUnit = unitType,
                Progression = progression,
                Count = count
            };
        }

        public InventoryActionConfig CreateItemAddAction(Id itemId, int count)
        {
            return new InventoryActionConfig(InventoryActionConfig.TypeInventoryAction.ItemAdd)
            {
                TypeItem = itemId,
                Count = count
            };
        }
        public InventoryActionConfig CreateItemSpendAction(Id itemId, int count)
        {
            return new InventoryActionConfig(InventoryActionConfig.TypeInventoryAction.ItemSpend)
            {
                TypeItem = itemId,
                Count = count
            };
        }
        public InventoryActionConfig CreateItemExpandLimitAction(Id itemId, int count)
        {
            return new InventoryActionConfig(InventoryActionConfig.TypeInventoryAction.ItemExpandLimit)
            {
                TypeItem = itemId,
                Count = count
            };
        }
    }
}