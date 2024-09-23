using Meta;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine;

namespace MetaUi
{
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
    public class MetaDemoRoot : MonoBehaviour, IUiEventHandler
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

        public void HandleEvent(IUiEvent uiEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}