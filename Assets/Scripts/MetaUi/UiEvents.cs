using AppRen;
using Meta.Configs;
using UnityEngine;

namespace MetaUi
{
    public struct UiEventRunAction : IUiEvent
    {
        public Id GroupId;
        public MetaActionConfig ActionConfig;

        public UiEventRunAction(Id groupId, MetaActionConfig actionConfig)
        {
            GroupId = groupId;
            ActionConfig = actionConfig;
        }
    }
    
    public struct UiEventSwitchScreen : IUiEvent
    {
        public readonly MetaScreenType ScreenType;
        public UiEventSwitchScreen(MetaScreenType screenType)
        {
            ScreenType = screenType;
        }
    }
    
    public struct UiEventTrainUnit : IUiEvent
    {
        public MetaActionConfig Action;
        public Transform UnitPivot;

        public UiEventTrainUnit(MetaActionConfig action, Transform unitPivot)
        {
            Action = action;
            UnitPivot = unitPivot;
        }
    }
}