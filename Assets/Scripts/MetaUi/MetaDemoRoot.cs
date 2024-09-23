using Meta;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MetaUi
{
    public interface IMessage
    {
    }
    
    public static class Extensions
    {
        public static void SendHierarchy<T, TK>(this T obj, TK message) where T : MonoBehaviour where TK : IMessage
        {
            ExecuteEvents.ExecuteHierarchy<IHierarchyHandler<TK>>(obj.transform.parent.gameObject, null,
                (handler, data) => handler.OnMessage(message));
        }
    }

    public interface IHierarchyHandler<in T> : IEventSystemHandler where T : IMessage
    {
        void OnMessage(T message);
    }




    /// <summary>
    ///  -Корень компоновки (тут)
    ///  - Презентационная логика (~)
    ///  - Модель домена
    ///  - Данные
    ///
    /// Во время исполнения все начинается с компоновки объекта.
    /// Корень компоновки собирает все независимые модули приложения.
    /// осуществляется в месте, где требуется интеграция различных модулей
    /// </summary>
    public class MetaDemoRoot : MonoBehaviour
    {
        [SerializeField] private MetaUiRoot _metaUi;
        private MetaModel _metaModel;
        
        protected void Awake()
        {
            //создание мета модели
            var gameConfig = new MetaConfigDevelopProvider();
            var emptyGameData = new MetaDto();
            var metaControllersFactory = new MetaControllersFactory(gameConfig.GetConfig()); 
            _metaModel = new MetaModel(gameConfig.GetConfig(), emptyGameData, metaControllersFactory);
            
            //создание мета UI

            //_metaUi.Setup();
        }

        protected void Update()
        {
            //UpdateMeta
            //UpdateUi
        }

  
    }
}