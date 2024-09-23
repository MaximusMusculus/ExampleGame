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
    public class MetaDemoRoot : MonoBehaviour
    {
        [SerializeField] private MetaTrainUnitsScreen _metaTrainUnits;
        
        
        private MetaModel _metaModel;
        
        protected void Awake()
        {
            //создание мета модели
            var gameConfig = new MetaConfigDevelopProvider().GetConfig();
            var emptyGameData = new MetaDto();
            var metaControllersFactory = new MetaControllersFactory(gameConfig); 
            _metaModel = new MetaModel(gameConfig, emptyGameData, metaControllersFactory);
            
            //создание мета UI
            var testActGroup = gameConfig.ActionsGroups[0];
            _metaTrainUnits.Setup(_metaModel.Inventory, _metaModel.Units, _metaModel.ConditionProcessor, _metaModel.ActionProcessor, testActGroup);
        }

        protected void Update()
        {
            //UpdateMeta
            //UpdateUi
        }

        private void HandleInputCommand()
        {
            //для проверки валидности выполнения, сейф мод, может запустить 2ю модель меты.
            //перед тем, как применять команды на основную модель, мы применим команды на проверочную. 
            //в случае эксепшена - можем что то сделать или вывести. Но, это скорее вариант для плавной отладки игры, чем стандартный механизм.
            //кстати, в случае ошибки, можем сбросить дамп стартового стейта и набора команд. Притом, это должно быть спрятано за интерфейсом
            //IInputCmdExecutor + Update.  Но это уже более высокий уровень.
        }


    }
}