using AppRen;
using Meta.Configs;

namespace Meta.TestConfiguration
{
    public interface IMetaConfigProvider
    {
        MetaConfig GetConfig();
    }

    //--
    public enum MapTestId : ushort
    {
        Hard = 11,
        Scrup = 12,
        Recruts = 13,

        Unit_1 = 22,
        Unit_2 = 23,
        Unit_3 = 24,
    }
    
    public static class MapEntityIdExtensions
    {
        public static Id Id(this MapTestId map)
        {
            return new Id((ushort) map);
        }
    }
    
    public class MetaConfigForTestGameplay : IMetaConfigProvider
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unit = new UnitConfigBuilder();
        private readonly ActionCollectionConfigBuilder _actionCollection = new ActionCollectionConfigBuilder();
        private readonly ConditionsConfigBuilder _conditions = new ConditionsConfigBuilder();
        private readonly MetaActionConfigBuilder _metaActions = new MetaActionConfigBuilder();

        public MetaConfig GetConfig()
        {
            return GetConfigForTests();
        }

        private MetaConfig GetConfigForTests()
        {
            _metaBuilder.NewConfig()
                .AddItemConfig(MapTestId.Hard.Id(), 50)
                .AddItemConfig(MapTestId.Scrup.Id(), 50, 500)
                .AddItemConfig(MapTestId.Recruts.Id(), 100, 150)
                .AddUnitConfig(_unit.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.Unit_2.Id()).SetCanUpgrade().Progression(1, 1, 1).Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.Unit_3.Id()).SetCanUpgrade().Progression(1, 2, 3).Build());

            //train unit action
            _metaBuilder.AddActionConfig(
                _metaActions.NewAction()
                    .SetActions(_actionCollection.NewAction()
                        .InventoryItemSpend(MapTestId.Scrup.Id(), 50)
                        .InventoryItemSpend(MapTestId.Recruts.Id(), 20)
                        .UnitAdd(_unit.NewUnit(MapTestId.Unit_1.Id()).Build(), 1)
                        .Build())
                    .SetRequire(_conditions.NewCollection(TypeCollection.And)
                        .InventoryItemHas(MapTestId.Scrup.Id(), 50)
                        .InventoryItemHas(MapTestId.Recruts.Id(), 20)
                        .Build())
                    .Build());

            return _metaBuilder.Build();
        }
    }



}