using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MetaUi
{
    public interface IUiEvent
    {
    }

    public interface IHierarchyHandler<in T> : IEventSystemHandler where T : IUiEvent
    {
        void HandleEvent(T message);
    }
    
    public static class UIExtensions
    {
        public static void SendHierarchy<T, TK>(this T obj, TK uiEvent) where T : MonoBehaviour where TK : IUiEvent
        {
            var isHandled = false;
            ExecuteEvents.ExecuteHierarchy<IHierarchyHandler<TK>>(obj.transform.parent.gameObject, null, (handler, data) =>
            {
                handler.HandleEvent(uiEvent);
                isHandled = true;
            });
            
            if (!isHandled)
            {
                throw new InvalidOperationException($"No handler found for event of type {typeof(TK).Name}");
            }
        }
    }
}