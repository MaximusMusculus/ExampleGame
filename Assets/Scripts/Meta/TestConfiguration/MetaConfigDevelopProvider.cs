using Meta.Configs;
using Meta.Configs.Conditions;

namespace Meta.TestConfiguration
{
    public class MetaConfigDevelopProvider : IMetaConfigProvider
    {
        private readonly MetaConfigBuilder _metaBuilder = new MetaConfigBuilder();
        private readonly UnitConfigBuilder _unit = new UnitConfigBuilder();
        private readonly MetaActionConfigBuilder _metaActions = new MetaActionConfigBuilder();
        private readonly MetaActionGroupConfigBuilder _actionGroup = new MetaActionGroupConfigBuilder();
        private readonly MetaQuestConfigBuilder _questBuilder = new MetaQuestConfigBuilder();
        private readonly ConditionsConfigBuilder _condition = new ConditionsConfigBuilder();


        public MetaConfig GetConfig()
        {
            return GetConfigForTests();
        }

        private MetaConfig GetConfigForTests()
        {
            _metaBuilder.NewConfig();

            _metaBuilder
                .AddItemConfig(MapTestId.Hard.Id(), 50)
                .AddItemConfig(MapTestId.Scrup.Id(), 5000, 5000)
                .AddItemConfig(MapTestId.Recruts.Id(), 740, 800)

                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitGunner.Id()).SetCanUpgrade().Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitScout.Id()).SetCanUpgrade().Progression(1, 1, 1).Build())
                .AddUnitConfig(_unit.NewUnit(MapTestId.UnitAssault.Id()).SetCanUpgrade().Progression(1, 2, 3).Build())

                .AddActionGroup(_actionGroup.New(MapTestId.GroupBarracs.Id())
                    .AddAction(_metaActions.NewAction().AddTrainUnit(MapTestId.UnitScout.Id(), 10, 50, 20).Build())
                    .AddAction(_metaActions.NewAction().AddTrainUnit(MapTestId.UnitGunner.Id(), 30, 100, 10).Build())
                    .AddAction(_metaActions.NewAction().AddTrainUnit(MapTestId.UnitAssault.Id(), 50, 150, 5).Build())
                    .SetDialogName("ViewBarracks")
                    .Build())


                .AddQuestConfig(_questBuilder
                    .NewCountQuest(MapTestId.QuestAddUnitGunner.Id(), MapTestId.UnitGunner.Id(), 10)
                    .SetTrigger(TypeQuest.UnitAdd)
                    .SetItemReward(MapTestId.Hard.Id(), 50)
                    .Build())

                .AddQuestConfig(_questBuilder
                    .NewCountQuest(MapTestId.QuestSpendRecruts.Id(), MapTestId.Recruts.Id(), 200)
                    .SetTrigger(TypeQuest.InventoryItemSpend)
                    .SetItemReward(MapTestId.Hard.Id(), 55)
                    .Build())

                .AddQuestConfig(_questBuilder.NewConditionalQuest(MapTestId.QuestCollectGunners.Id(), 
                        _condition
                            .NewCollection(TypeCollection.And)
                            .UnitCountCondition(MapTestId.UnitGunner.Id(), TypeCompare.GreaterOrEqual, 5)
                            .Build())
                    .SetTrigger(TypeQuest.UnitAdd)
                    .SetItemReward(MapTestId.Recruts.Id(), 100)
                    .Build());



            return _metaBuilder.Build();
        }
    }
}