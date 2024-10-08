using AppRen;
using Meta.Configs;
using Meta.Configs.Conditions;
using UnityEngine;

namespace Meta.TestConfiguration
{
    public class MetaConfigProviderTestBig : IMetaConfigProvider
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unit = new UnitConfigBuilder();
        private readonly ActionCollectionConfigBuilder _actionCollection = new ActionCollectionConfigBuilder();
        private readonly ConditionsConfigBuilder _conditions = new ConditionsConfigBuilder();
        private readonly MetaActionConfigBuilder _metaActions = new MetaActionConfigBuilder();
        private readonly MetaActionGroupConfigBuilder _actionGroup = new MetaActionGroupConfigBuilder();
        
        
        private readonly IIdProvider _idProvider = new IdProvider(new IdProviderDto {nextId = 100});
        

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
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitGunner.Id()).SetCanUpgrade().Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitScout.Id()).SetCanUpgrade().Progression(1, 1, 1).Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitAssault.Id()).SetCanUpgrade().Progression(1, 2, 3).Build());

            _actionGroup.New(MapTestId.GroupBarracs.Id());
            //train unit action
            _actionGroup.AddAction(_metaActions.NewAction()
                .SetActions(_actionCollection.NewAction()
                    .InventoryItemSpend(MapTestId.Scrup.Id(), 50)
                    .InventoryItemSpend(MapTestId.Recruts.Id(), 20)
                    .UnitAdd(_unit.NewUnit(MapTestId.UnitGunner.Id()).Build(), 1)
                    .Build())
                .SetRequire(_conditions.NewCollection(TypeCollection.And)
                    .UnitCountCondition(MapTestId.UnitGunner.Id(), TypeCompare.Less, 10)
                    .Build())
                .Build());

            //spend
            _actionGroup.AddAction(_metaActions.NewAction()
                .SetActions(_actionCollection.NewAction()
                    .InventoryItemAdd(MapTestId.Scrup.Id(), 50)
                    .InventoryItemAdd(MapTestId.Recruts.Id(), 20)
                    .UnitSpend(_unit.NewUnit(MapTestId.UnitGunner.Id()).Build(), 1)
                    .Build())
                .Build());


            //resoures
            for (int i = 0; i < 1000; i++)
            {
                _metaBuilder.AddItemConfig(_idProvider.GetId(), i, i);
            }

            //units
            var unitIdRange = new Vector2Int(0, 0);
            for (var i = 0; i < 1000; i++)
            {
                var unitId = _idProvider.GetId();
                _metaBuilder.AddUnitConfig(_unit.NewUnit(unitId).SetCanUpgrade().Build());

                if (i == 0)
                {
                    unitIdRange.x = unitId;
                }

                unitIdRange.y = unitId;
            }

            //actions
            for (int i = 0; i < 1000; i++)
            {
                var addAllUnitsAction = _actionCollection.NewAction();
                for (var unitId = unitIdRange.x; unitId < unitIdRange.y; unitId++)
                {
                    addAllUnitsAction.UnitAdd(_unit.NewUnit((ushort) unitId).Build(), 1);
                }

                _actionGroup.AddAction(_metaActions.NewAction()
                    .SetActions(addAllUnitsAction.Build())
                    .Build());
            }


            _metaBuilder.AddActionGroup(_actionGroup.Build());
            return _metaBuilder.Build();
        }
    }
}