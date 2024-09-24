using UnityEngine;

namespace MetaUi
{
    public enum MetaScreenType
    {
        None,
        Units,
        Play
    }
    public struct UiEventSwitchScreen : IUiMessage
    {
        public readonly MetaScreenType ScreenType;
        public UiEventSwitchScreen(MetaScreenType screenType)
        {
            ScreenType = screenType;
        }
    }
    
    public class MetaMenuBar  : MonoBehaviour
    {
        public void Setup()
        {
            
        }

        public void OnUnitsClick()
        {
            this.SendHierarchy(new UiEventSwitchScreen(MetaScreenType.Units));
        }

        public void OnPlayClick()
        {
            this.SendHierarchy(new UiEventSwitchScreen(MetaScreenType.Play));
        }
    }
}