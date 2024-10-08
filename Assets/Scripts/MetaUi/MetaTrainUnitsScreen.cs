using Meta;
using Meta.Configs;
using Meta.Controllers;
using UnityEngine;

namespace MetaUi
{
    /// <summary>
    /// Тут нужны юниты, ресурсы, действия и кондишены.  
    /// </summary>
    public class MetaTrainUnitsScreen : MonoBehaviour, IHierarchyHandler<UiEventTrainUnit>
    {
        [SerializeField] private MetaTrainUnits _metaTrainUnits;
        [SerializeField] private MetaItemsBar _metaItemsBar;
        
        private MetaModel _metaModel;
        private MetaActionsGroupConfig _actionsGroup;
        private IConditionProcessor _conditionProcessor;
        
        public MetaTrainUnitsScreen Setup(MetaModel metaModel, MetaActionsGroupConfig actions, ISpriteHolderTest spriteHolder)
        {
            _actionsGroup = actions;
            _conditionProcessor = metaModel.ConditionProcessor;
            _metaItemsBar.Setup(metaModel.Inventory, spriteHolder);
            _metaTrainUnits.Setup(metaModel, actions.Actions, spriteHolder);
            return this;
        }
        
        public void UpdateView()
        {
            _metaItemsBar.UpdateItems();
            _metaTrainUnits.UpdateUnits();
        }

        public void HandleEvent(UiEventTrainUnit message)
        {
            if (_conditionProcessor.Check(message.Action.Require))
            {
                this.SendHierarchy(new UiEventRunAction(_actionsGroup.TypeGroup, message.Action));
                
                //знаем какого юнита трерируем - знаем его стоимость и тд. Знаем позицию иконки юнита, знаем бар ресурсов и бар армии. 
                //можем выполнить красивую анимацию с синхронизацией, естественно этого тут нет ;)
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