using Meta.Configs;
using Meta.Configs.Conditions;
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
                .AddItemConfig(MapTestId.Scrup.Id(), 0,500)
                .AddItemConfig(MapTestId.Recruts.Id(), 0, 500)
        
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
                    //тут будут зависимости на открытие. Проверку на возможность делать то или иное буду проводить во 2й модели 
                    .SetRequire(_conditions.NewCollection(TypeCollection.And)
                        .InventoryItemHas(MapTestId.Scrup.Id(), 50)
                        .InventoryItemHas(MapTestId.Recruts.Id(), 20)
                        .UnitCountCondition(MapTestId.Unit_1.Id(), TypeCompare.Less, 10)
                        .Build())
                    .Build());
            
            
            //add resourse and costUnitAction
            _metaBuilder.AddActionConfig(
                _metaActions.NewAction()
                    .SetActions(_actionCollection.NewAction()
                        .InventoryItemAdd(MapTestId.Scrup.Id(), 50)
                        .InventoryItemAdd(MapTestId.Recruts.Id(), 20)
                        .UnitSpend(_unit.NewUnit(MapTestId.Unit_1.Id()).Build(), 1)
                        .Build())
                    .Build());
            
            return _metaBuilder.Build();
        }
    }
}