using System;
using Meta;
using Meta.Models;
using Meta.TestConfiguration;
using UnityEngine;

namespace MetaUi
{
    public interface ITestInputCmdExecutor
    {
        void Execute(UiEventRunAction command);
    }
    
    public class MetaDemoInitializer : MonoBehaviour, ITestInputCmdExecutor  //для теста
    {
        [SerializeField] private MetaUiRoot _metaUi;
        private MetaModel _metaModel;
        
        protected void Awake()
        {
            var gameConfig = new MetaConfigDevelopProvider().GetConfig();
            var emptyGameData = new MetaDto();
            var metaControllersFactory = new MetaControllersFactory(gameConfig); 
            _metaModel = new MetaModel(gameConfig, emptyGameData, metaControllersFactory);
            
          
            
            _metaUi.Setup(_metaModel, this);
        }
        
        public void Execute(UiEventRunAction command)
        {
            try
            {
                _metaModel.Process(command.ActionConfig.Actions);
            }
            catch (Exception e)
            {
                //тут можно записать накопенные команды в лог, чтобы потом воспроизвести
                //имея конфиг, стартовый стейт и набор команд со временем их применения, можем воспроизводить ошибку.

                //вместо моментального выполнения, можно создавать команды. Вести историю и т.д.
                //для проверки валидности выполнения, сейф мод, может запустить 2ю модель меты.
                //перед тем, как применять команды на основную модель, мы применим команды на проверочную. 
                //в случае ошибки, можем сбросить дамп стартового стейта и набора команд, а игроку сказать - что не можем это выполнить. НО не ронять игру.
                //IInputCmdExecutor + Update. Но это уже более высокий уровень
                Debug.LogError(e);
            }
        }
        
    }
}