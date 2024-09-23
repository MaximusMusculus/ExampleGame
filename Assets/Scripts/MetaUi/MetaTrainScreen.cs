using Meta.Configs;
using Meta.Controllers;
using UnityEngine;


namespace MetaUi
{
    
    /// <summary>
    /// Тут нужны юниты, ресурсы, действия и кондишены.  
    /// </summary>
    public class MetaTrainScreen : MonoBehaviour, IHierarchyHandler<TrainUiEvent>
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

        public void OnMessage(TrainUiEvent message)
        {
           Debug.Log("MetaTrainScreen train unit" + message.UnitType);
        }
    }
}