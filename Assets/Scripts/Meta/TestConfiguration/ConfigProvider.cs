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

        UnitGunner = 22,
        UnitScout = 23,
        UnitAssault = 24,
        
        GroupBarracs = 31, //barracks
        
        QuestSpendRecruts = 41,
        QuestAddUnitGunner = 43,
        QuestCollectGunners = 47,
    }
    
    public static class MapEntityIdExtensions
    {
        public static Id Id(this MapTestId map)
        {
            return new Id((ushort) map);
        }
    }
}