using System.Collections.Generic;
using System.Linq;
using Meta;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers;
using UnityEngine;

namespace MetaUi
{
    public static class ActionCollectionExtension
    {
        private static readonly UnitActionCollection UnitActionCollection = new UnitActionCollection(5);
        private static readonly InventoryActionItemCollection InventoryActionItemCollection = new InventoryActionItemCollection(10);
        private static readonly ActionSplitProcessor ActionSplitProcessor = new ActionSplitProcessor();

        public static bool HasUnitAction(this IActionCollectionConfig actionCollection)
        {
            ActionSplitProcessor.Reset().Set(UnitActionCollection).Fill(actionCollection);
            return UnitActionCollection.Units.Length > 0;
        }
        public static bool HasItemAction(this IActionCollectionConfig actionCollection)
        {
            ActionSplitProcessor.Reset().Set(InventoryActionItemCollection).Fill(actionCollection);
            return InventoryActionItemCollection.Items.Count > 0;
        }

    }
    
    
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
            return actions.Actions.Where(trainAction => trainAction.Actions.HasUnitAction());
        }
        
        public void UpdateView()
        {
            _metaItemsBar.UpdateView();
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
                _metaItemsBar.UpdateView();
            }
            else
            {
                Debug.Log("Can't train unit");
            }
        }


    }
}