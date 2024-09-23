using Meta.Configs;
using Meta.Controllers;
using UnityEngine;

namespace MetaUi
{
    /// <summary>
    /// Тут нужны юниты, ресурсы, действия и кондишены.  
    /// </summary>
    public class MetaTrainUnitsScreen : MonoBehaviour, IHierarchyHandler<TrainUiEvent>
    {
        private IInventory _items;            //topBar
        private IUnits _units;                //unitsContent
        private IConditionProcessor _conditionProcessor;//checkRequire
        private IActionProcessor _actionsProcessor;

        [SerializeField] private MetaTrainUnits _metaTrainUnits;
        //topBar
        //bottomBar
        
        public MetaTrainUnitsScreen Setup(IInventory items, IUnits units, IConditionProcessor conditions, IActionProcessor actionsProcessor, MetaActionsGroupConfig actions)
        {
            _items = items;
            _units = units;
            _conditionProcessor = conditions;
            _actionsProcessor = actionsProcessor;
            _metaTrainUnits.Setup(items, units, conditions, actions.Actions);
            return this;
        }

        public void HandleMessage(TrainUiEvent message)
        {
            if (_conditionProcessor.Check(message.Action.Require))
            {
                //пока пробую менять тут, но будет проброс сообщения с командой наверх.
                _actionsProcessor.Process(message.Action.Actions);
                _metaTrainUnits.ShowUnits();
            }
            else
            {
                Debug.Log("Can't train unit");
            }
        }
    }
}