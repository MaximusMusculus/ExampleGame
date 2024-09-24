using System;
using UnityEngine;

namespace MetaUi
{
    public class MetaUiRoot : MonoBehaviour, IHierarchyHandler <IUiMessage>
    {
        public void HandleMessage(IUiMessage uiMessage)
        {
            throw new ArgumentException("Not handled message type: " + uiMessage.GetType());
        }
        
        
        
        
        
    }
}