using Meta.Configs;
using Meta.TestConfiguration;

namespace Meta.Tests.Editor.Controllers
{
    public class TestMetaConfigProvider : IMetaConfigProvider
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unit = new UnitConfigBuilder();
        private readonly ActionCollectionConfigBuilder _actionCollection = new ActionCollectionConfigBuilder();
        private readonly MetaActionConfigBuilder _metaActions = new MetaActionConfigBuilder();
        private readonly ConditionsConfigBuilder _conditions = new ConditionsConfigBuilder();
        
        public MetaConfig GetConfig()
        {
            return GetConfigForTests();
        }

        private MetaConfig GetConfigForTests()
        {
            _metaBuilder.NewConfig()
            
                .AddItemConfig(MapTestId.Hard.Id(), 0)
                .AddItemConfig(MapTestId.Scrup.Id(), 0, 100)
                .AddItemConfig(MapTestId.Recruts.Id(), 0, 100)
        
                .AddUnitConfig(_unit.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.Unit_2.Id()).SetCanUpgrade().Progression(1, 1, 1).Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.Unit_3.Id()).SetCanUpgrade().Progression(1, 2, 3).Build());
            
            
            //train unit action
            _metaBuilder.AddActionConfig( 
                _metaActions.NewAction()
                    .SetSpend(_actionCollection.NewAction()
                        .InventoryItemSpend(MapTestId.Scrup.Id(), 50)
                        .InventoryItemSpend(MapTestId.Recruts.Id(), 20)
                        .Build())
                    .SetAdd(_actionCollection.NewAction()
                        .UnitAdd(_unit.NewUnit(MapTestId.Unit_1.Id()).Build(), 1)
                        .Build())
                    .Build());
            
            return _metaBuilder.Build();
        }
    }
}