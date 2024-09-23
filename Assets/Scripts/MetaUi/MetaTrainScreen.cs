using Meta.Configs;
using Meta.Controllers;
using UnityEngine;


namespace MetaUi
{
    public interface IUiEvent { }
    public interface IUiEventHandler
    {
        void HandleEvent(IUiEvent uiEvent);
    }
    public interface IUiCustomEventHandler<in TEvent> where TEvent : IUiEvent
    {
        void Handle(TEvent uiEvent);
    }
    
    
    
    
    /// <summary>
    /// Тут нужны юниты, ресурсы, действия и кондишены.  
    /// </summary>
    public class MetaTrainScreen : MonoBehaviour
    {
        private IInventory _items;            //topBar
        private IUnits _units;                //unitsContent
        private IConditionProcessor _conditionProcessor;//checkRequire
        private MetaActionsGroupConfig _actionsGroup;   //config
        
        public MetaTrainScreen Setup(IInventory items, IUnits units, IConditionProcessor conditions, MetaActionsGroupConfig actions)
        {
            _items = items;
            _units = units;
            _conditionProcessor = conditions;
            _actionsGroup = actions;
            return this;
        }
        
        
    }
}