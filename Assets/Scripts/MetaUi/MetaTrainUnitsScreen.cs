using System.Collections.Generic;
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
            _metaTrainUnits.OneTimeSetup(metaModel, SelectTrainActions(actions), spriteHolder);
            return this;
        }
        
        private IEnumerable<MetaActionConfig> SelectTrainActions(MetaActionsGroupConfig actions)
        {
            //отбираем только те действия, которые добавляют юнитов
            //если будет такое часто - передать фильтр
            foreach (var trainAction in actions.Actions)
            {
                foreach (var action in trainAction.Actions.GetAll())
                {
                    //меня интересует конкретная группа действий
                    if (action.ActionGroup == TypeActionGroup.Units)
                    {
                        yield return trainAction;
                    }
                }
            }
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