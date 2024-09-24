using Meta;
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
        private IConditionProcessor _conditionProcessor;//checkRequire
        private IActionProcessor _actionsProcessor;

        [SerializeField] private MetaTrainUnits _metaTrainUnits;
        [SerializeField] private MetaItemsBar _metaItemsBar;
        private MetaModel _metaModel;
        
        public MetaTrainUnitsScreen Setup(MetaModel metaModel, MetaActionsGroupConfig actions, ISpriteHolderTest spriteHolder)
        {
            _metaItemsBar.Setup(metaModel.Inventory, spriteHolder);
            _conditionProcessor = metaModel.ConditionProcessor;
            _actionsProcessor = metaModel.ActionProcessor;
            _metaTrainUnits.Setup(metaModel, actions.Actions, spriteHolder);
            return this;
        }

        public void HandleMessage(TrainUiEvent message)
        {
            if (_conditionProcessor.Check(message.Action.Require))
            {
                //пока пробую менять тут, но будет проброс сообщения с командой наверх.
                _actionsProcessor.Process(message.Action.Actions);
                
                //синхронизация анимаций
                _metaTrainUnits.UpdateUnits();
                _metaItemsBar.UpdateItems();
            }
            else
            {
                Debug.Log("Can't train unit");
            }
        }
    }
}