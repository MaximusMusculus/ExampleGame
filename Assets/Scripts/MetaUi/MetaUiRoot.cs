using UnityEngine;

namespace MetaUi
{
    public class MetaUiRoot : MonoBehaviour, IHierarchyHandler <IMessage>
    {
        public void OnMessage(IMessage message)
        {
            //throw new System.NotImplementedException();
        }
    }
}