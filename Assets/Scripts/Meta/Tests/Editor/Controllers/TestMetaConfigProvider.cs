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
        private readonly MetaActionGroupConfigBuilder _actionGroup = new MetaActionGroupConfigBuilder();
        
        public MetaConfig GetConfig()
        {
            return GetConfigForTests();
        }

        private MetaConfig GetConfigForTests()
        {
            _metaBuilder.NewConfig()

                .AddItemConfig(MapTestId.Hard.Id(), 0)
                .AddItemConfig(MapTestId.Scrup.Id(), 0, 500)
                .AddItemConfig(MapTestId.Recruts.Id(), 0, 500)

                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitGunner.Id()).SetCanUpgrade().Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitScout.Id()).SetCanUpgrade().Progression(1, 1, 1).Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitAssault.Id()).SetCanUpgrade().Progression(1, 2, 3).Build());


            //train unit action
            _actionGroup.New(MapTestId.GroupBarracs.Id());
            _actionGroup.AddAction(_metaActions.NewAction()
                .SetActions(_actionCollection.NewAction()
                    .InventoryItemSpend(MapTestId.Scrup.Id(), 50)
                    .InventoryItemSpend(MapTestId.Recruts.Id(), 20)
                    .UnitAdd(_unit.NewUnit(MapTestId.UnitGunner.Id()).Build(), 1)
                    .Build())
                //тут будут зависимости на открытие. Проверку на возможность делать то или иное буду проводить во 2й модели 
                .SetRequire(_conditions.NewCollection(TypeCollection.And)
                    .InventoryItemHas(MapTestId.Scrup.Id(), 50)
                    .InventoryItemHas(MapTestId.Recruts.Id(), 20)
                    .UnitCountCondition(MapTestId.UnitGunner.Id(), TypeCompare.Less, 10)
                    .Build())
                .Build());

            //add resourse and costUnitAction
            _actionGroup.AddAction(_metaActions.NewAction()
                .SetActions(_actionCollection.NewAction()
                    .InventoryItemAdd(MapTestId.Scrup.Id(), 50)
                    .InventoryItemAdd(MapTestId.Recruts.Id(), 20)
                    .UnitSpend(_unit.NewUnit(MapTestId.UnitGunner.Id()).Build(), 1)
                    .Build())
                .Build());
            
            _metaBuilder.AddActionGroup(_actionGroup.Build());
            return _metaBuilder.Build();
        }
    }
}