using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Controllers;
using Meta.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace MetaUi
{
    public class TrainElemData
    {
        public string Title;
        public string Description;
        public string CountAndLimit;
        
        public bool ButtonEnabled;
        public Sprite Icon;

        public MetaActionConfig ActionConfig;

        public List<ItemViewData> ItemsData = new List<ItemViewData>() {new ItemViewData(), new ItemViewData(), new ItemViewData()};
        
        public class ItemViewData
        {
            public bool IsEnable;
            public Sprite Icon;
            public string Text;
        }
        
        public void Reset()
        {
            Title = string.Empty;
            Description = string.Empty;
            CountAndLimit = string.Empty;
            ButtonEnabled = false;
            Icon = null;
            foreach (var itemViewData in ItemsData)
            {
                itemViewData.IsEnable = false;
                itemViewData.Icon = null;
                itemViewData.Text = string.Empty;
            }
        }
    }
    
    public enum TypeAction
    {
        None,
        Add,
        Spend,
        ExpandLimit
    }
    public class ItemChangeInfo
    {
        public Id ItemId { get; private set; }
        public int Count{ get; private set; }
        public TypeAction TypeAction { get; private set; }
        public bool IsEmpty { get; private set; }
        
        public void Set(Id itemId, int count, TypeAction action)
        {
            ItemId = itemId;
            Count = count;
            TypeAction = action;
            IsEmpty = false;
        }
        
        public void Reset()
        {
            IsEmpty = true;
            TypeAction = TypeAction.None;
            ItemId = Id.Empty;
            Count = 0;
        }
    }
    public class InventoryActionItemCollection: IInventoryActionVisitor
    {
        private int _index;
        public IReadOnlyList<ItemChangeInfo> Items { get; }
        
        public int Count { get; private set; }
        
        public InventoryActionItemCollection(int maxSize)
        {
            var items = new List<ItemChangeInfo>(maxSize);
            for (int i = 0; i < maxSize; i++)
            {
                items.Add(new ItemChangeInfo());
            }
            Items = items;
        }

        public void Reset()
        {
            _index = 0;
            foreach (var viewData in Items)
            {
                viewData.Reset();
            }
        }

        public void ItemAdd(Id itemId, int count)
        {
            Fill(itemId, count, TypeAction.Add);
        }
        public void ItemSpend(Id itemId, int count)
        {
            Fill(itemId, count, TypeAction.Spend);
        }
        public void ItemExpandLimit(Id itemId, int count)
        {
            Fill(itemId, count, TypeAction.ExpandLimit);
        }

        private void Fill(Id item, int count, TypeAction action)
        {
            Items[_index].Set(item, count, action);
            _index++;
            Count = _index + 1;
        }
    }


    public class UnitChangeInfo
    {
        public Id UnitType;
        public int Count;
        public UnitProgressionDto Progression;
        public TypeAction Action;
    }
    public class UnitActionCollection : IUnitActionVisitor
    {
        private int _index;
        public IReadOnlyList<UnitChangeInfo> Units { get; }
        public UnitActionCollection(int maxSize)
        {
            var units = new List<UnitChangeInfo>(maxSize);
            for (int i = 0; i < maxSize; i++)
            {
                units.Add(new UnitChangeInfo());
            }
            Units = units;
        }
        
        public void Reset()
        {
            _index = 0;
            foreach (var viewData in Units)
            {
                viewData.UnitType = Id.Empty;
                viewData.Count = 0;
                viewData.Progression = null;
                viewData.Action = TypeAction.None;
            }
        }
        
        public void UnitAdd(Id typeUnit, UnitProgressionDto progression, int count)
        {
            Fill(typeUnit, progression, count, TypeAction.Add);
        }
        public void UnitSpend(Id typeUnit, UnitProgressionDto progression, int count)
        {
            Fill(typeUnit, progression, count, TypeAction.Spend);
        }
        
        private void Fill(Id typeUnit, UnitProgressionDto progression, int count, TypeAction action)
        {
            var data = Units[_index];
            data.UnitType = typeUnit;
            data.Progression = progression;
            data.Count = count;
            data.Action = action;
            
            _index++;
        }
    }
    
    
    /// <summary>
    /// Получает из любого действия список предметов, которые будут изменены
    /// </summary>
    public class ActionSplitProcessor : IActionVisitor
    {
        private InventoryActionItemCollection _items;
        private UnitActionCollection _units;

        public ActionSplitProcessor Reset()
        {
            _items = null;
            _units = null;
            return this;
        }
        public ActionSplitProcessor Set(InventoryActionItemCollection items)
        {
            Assert.IsNull(_items);
            _items = items;
            return this;
        }
        public ActionSplitProcessor Set(UnitActionCollection units)
        {
            Assert.IsNull(_units);
            _units = units;
            return this;
        }
        
        public void Fill(IActionCollectionConfig actionConfig)
        {
            _items.Reset();
            _units.Reset();

            foreach (var action in actionConfig.GetAll())
            {
                action.Visit(this);
            }
        }
        
        public void Visit(IInventoryAction inventoryActionConfig)
        {
           inventoryActionConfig?.Visit(_items);
        }
        public void Visit(IUnitAction unitActionConfig)
        {
            unitActionConfig?.Visit(_units);
        }
    }

    

    public class TrainActionDataPresenter
    {
        private readonly InventoryActionItemCollection _price;
        private readonly UnitActionCollection _units;
        private readonly ISpriteHolderTest _spriteHolder;
        private readonly int _maxPriceItemsSize = 3;
        private readonly ActionSplitProcessor _actionSplitProcessor;
        
        public TrainActionDataPresenter(ISpriteHolderTest spriteHolder, ActionSplitProcessor actionSplitProcessor)
        {
            _spriteHolder = spriteHolder;
            _actionSplitProcessor = actionSplitProcessor;
            _price = new InventoryActionItemCollection(_maxPriceItemsSize);
            _units = new UnitActionCollection(1);
        }
        
        public void ActionToView(MetaActionConfig actionConfig, TrainElemData elemData, IUnits units)
        {
            elemData.Reset();
            elemData.ActionConfig = actionConfig;
            _actionSplitProcessor.Reset().Set(_price).Set(_units).Fill(actionConfig.Actions);

            var unit = _units.Units[0];
            elemData.Title = "UnitType: " + unit.UnitType;
            elemData.Description = "Description: ";
            elemData.Icon = _spriteHolder.GetSprite(unit.UnitType);

            var existCount = units.TryGetUnit(unit.UnitType, unit.Progression, out var existUnit) ? existUnit.Count : 0;
            elemData.CountAndLimit = $"{existCount}"; //{units.GetLimit(unit.UnitType, unit.Progression)}";

            
            Assert.IsTrue(_price.Count <= elemData.ItemsData.Count);
            for (var i = 0; i < _price.Items.Count; i++)
            {
                var item = _price.Items[i];
                var itemViewInfo = elemData.ItemsData[i];
                itemViewInfo.Icon = _spriteHolder.GetSprite( item.ItemId);
                itemViewInfo.Text = item.Count.ToString();
                itemViewInfo.IsEnable = !item.IsEmpty;
            }
        }
        
        
        public void Visit(IInventoryAction inventoryActionConfig)
        {
            inventoryActionConfig.Visit(_price);
        }
        public void Visit(IUnitAction unitActionConfig)
        {
            unitActionConfig.Visit(_units);
        }
    }
    

    /// <summary>
    /// знает про юнитов их количество
    /// знает, можно ли купить юнита
    /// знает про список юнитов и их данные
    /// </summary>
    public class MetaTrainUnits : MonoBehaviour
    {
        private IUnits _unitsController;
        private IConditionProcessor _conditions;
        private IEnumerable<MetaActionConfig> _trainActions;
        
        private List<TrainElemData> _elemsData;
        [SerializeField] private List<MetaTrainUnit> _elemsView;
        
        private IActionProcessor _actionProcessor;
        private TrainActionDataPresenter _trainDataPresenter;
        
        public void OneTimeSetup(MetaModel metaModel, IEnumerable<MetaActionConfig> trainActions, ISpriteHolderTest spriteHolder)
        {
            Assert.IsNull(_trainDataPresenter);
            
            _trainDataPresenter = new TrainActionDataPresenter(spriteHolder, new ActionSplitProcessor());
            _unitsController = metaModel.Units;
            _conditions = metaModel.ConditionProcessor;
            _trainActions = trainActions;
            BindDataToView();
            UpdateUnits();
        }
        
        private void BindDataToView()
        {
            _elemsData = new List<TrainElemData>(_elemsView.Count);
            foreach (var trainElem in _elemsView)
            {
                var viewData = new TrainElemData();
                _elemsData.Add(viewData);
                trainElem.SetData(viewData);
            }
        }

        public void UpdateUnits()
        {
            int i = 0;
            foreach (var trainAction in _trainActions)
            {
                if (i > _elemsData.Count-1)
                {
                    break;
                }
                _trainDataPresenter.ActionToView(trainAction, _elemsData[i], _unitsController);
                i++;
            }

            for (var j = 0; j < _elemsView.Count; j++)
            {
                if (j < i)
                {
                    _elemsView[j].RefreshElem();
                }
                else
                {
                    _elemsView[j].gameObject.SetActive(false);
                }
            }
        }
        
        
        private void FillTrainElemView(MetaActionConfig actionConfig, TrainElemData elemData)
        {
            elemData.ActionConfig = actionConfig;
            //первая, мучительная процедура распаковки)) экшена.  Далее - либо, буду делать типизированные (классов что ли жалко)
            //либо спец адаптеры.
            
            //если захочется тутор, то кондишен оборачивается доп-но в тутор логику
            elemData.ButtonEnabled = _conditions.Check(actionConfig.Require);


            // я знаю, что в списке действий - должно быть действие по добавлению юнита
            var unitAction = actionConfig.Actions.GetAll().FirstOrDefault(s => s.ActionGroup == TypeActionGroup.Units) as IUnitAction;
            Assert.IsNotNull(unitAction);

            throw new NotImplementedException();

            /*elemData.Title = "UnitType: " + unitAction.TypeUnit;
            elemData.Description = "Description: ";
            elemData.Icon = _spriteHolder.GetSprite(unitAction.TypeUnit);

            var unitCount = 0;
            var unitLimit = int.MaxValue;
            
            if(_unitsController.TryGetUnit(unitAction.TypeUnit, unitAction.Progression, out var unit))
            {
                unitCount = unit.Count;
            }
            
            // actionConfig.Actions.GetEnumerator()  тут  мне надо обойти список действий, отобрать только те, что снимают ресурс
            // попробовать вывести это в представление.  Так же проверить, есть ли данный ресурс в нужном колве. Если нет - покрасить интерфейс, залочить кнопку
            //такие вещи будут происходить достаточно часто. 


            Assert.IsTrue(actionConfig.Actions.Items.Count <= elemData.ItemsData.Count);
            foreach (var data in elemData.ItemsData)
            {
                data.IsEnable = false;
            }
            int i = 0;
            foreach (var actionsItem in actionConfig.Actions.Items)
            {
                var priceData = elemData.ItemsData[i];
                priceData.Icon = _spriteHolder.GetSprite(actionsItem.TypeItem);
                priceData.Text = actionsItem.Count.ToString();
                priceData.IsEnable = true;
                i++;
            }
            




            //ищу условие на лимит юнитов
            foreach (var condition in actionConfig.Require.GetConditions())
            {
                if (condition.TypeCondition == TypeCondition.UnitsCount)
                {
                    //подумать, как убрать кастинг
                    unitLimit = ((CountConditionConfig) condition).Value;
                }
            }
            elemData.CountAndLimit = $"{unitCount}/{unitLimit}";*/
        }


    }
}