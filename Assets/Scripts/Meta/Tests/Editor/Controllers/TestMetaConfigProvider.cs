using Meta.Configs;
using Meta.TestConfiguration;

namespace Meta.Tests.Editor.Controllers
{
    public class TestMetaConfigProvider : IMetaConfigProvider
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unit = new UnitConfigBuilder();
        private readonly ActionConfigBuilder _action = new ActionConfigBuilder();
        
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
            
            return _metaBuilder.Build();
        }
    }
}