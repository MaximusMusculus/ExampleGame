using AppRen;
using Meta.Configs.Actions;
using Meta.Models;

namespace Meta.Configs.TestConfiguration
{
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
    
    
    //--
    public interface IMetaConfigProvider
    {
        MetaConfig GetConfig();
    }
    public class MetaConfigForTestGameplay : IMetaConfigProvider
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unitBuilder = new UnitConfigBuilder();

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
                .AddUnitConfig(_unitBuilder.NewUnit(MapTestId.Unit_1.Id()).SetCanUpgrade().Build())
                .AddUnitConfig(_unitBuilder.NewUnit(MapTestId.Unit_2.Id()).SetCanUpgrade().Progression(1, 1, 1).Build())
                .AddUnitConfig(_unitBuilder.NewUnit(MapTestId.Unit_3.Id()).SetCanUpgrade().Progression(1, 2, 3).Build());
            return _metaBuilder.Build();
        }
    }



}