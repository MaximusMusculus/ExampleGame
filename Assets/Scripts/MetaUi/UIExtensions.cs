using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MetaUi
{
    public interface IUiMessage
    {
    }

    public interface IHierarchyHandler<in T> : IEventSystemHandler where T : IUiMessage
    {
        void HandleMessage(T message);
    }
    
    public static class UIExtensions
    {
        public static void SendHierarchy<T, TK>(this T obj, TK message) where T : MonoBehaviour where TK : IUiMessage
        {
            var isHandled = false;
            ExecuteEvents.ExecuteHierarchy<IHierarchyHandler<TK>>(obj.transform.parent.gameObject, null, (handler, data) =>
            {
                handler.HandleMessage(message);
                isHandled = true;
            });
            
            if (!isHandled)
            {
                throw new InvalidOperationException($"No handler found for message of type {typeof(TK).Name}");
            }
        }
    }
}