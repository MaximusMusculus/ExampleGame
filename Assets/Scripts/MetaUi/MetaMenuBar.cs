using UnityEngine;

namespace MetaUi
{
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