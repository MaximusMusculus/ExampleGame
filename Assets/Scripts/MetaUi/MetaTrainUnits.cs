using System.Collections.Generic;
using AppRen;
using Meta;
using Meta.Configs;
using Meta.Configs.Actions;
using Meta.Configs.Conditions;
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
            public bool IsWarning;
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
            for (var i = 0; i < Items.Count; i++)
            {
                Items[i].Reset();
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
            Count = _index;
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
        public UnitChangeInfo[] Units { get; }
        public UnitActionCollection(int maxSize)
        {
            Units = new UnitChangeInfo[maxSize];
            for (var i = 0; i < maxSize; i++)
            {
                Units[i] = new UnitChangeInfo();
            }
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
        private readonly UnitActionCollection _unitsCollection;
        private readonly ISpriteHolderTest _spriteHolder;
        private readonly int _maxPriceItemsSize = 3;
        private readonly ActionSplitProcessor _actionSplitProcessor;

        private readonly IUnits _units;
        private readonly IInventory _inventory;
        private readonly IConditionProcessor _conditionProcessor;
        
        //Класс содержжит логику проверки и логику заполнения вьюхи
        public TrainActionDataPresenter(ISpriteHolderTest spriteHolder, IUnits units, IInventory inventory, IConditionProcessor conditionProcessor)
        {
            _conditionProcessor = conditionProcessor;
            _spriteHolder = spriteHolder;
            _inventory = inventory;
            _units = units;

            _actionSplitProcessor = new ActionSplitProcessor();
            _price = new InventoryActionItemCollection(_maxPriceItemsSize);
            _unitsCollection = new UnitActionCollection(1);
        }
        
        public void ActionToView(MetaActionConfig actionConfig, TrainElemData viewData)
        {
            viewData.Reset();
            viewData.ActionConfig = actionConfig;
            _actionSplitProcessor.Reset().Set(_price).Set(_unitsCollection).Fill(actionConfig.Actions);

            var unit = _unitsCollection.Units[0];
            viewData.Title = "UnitType: " + unit.UnitType;
            viewData.Description = "Description: ";
            viewData.Icon = _spriteHolder.GetSprite(unit.UnitType);

            Assert.IsTrue(_price.Count <= viewData.ItemsData.Count);
            foreach (var itemView in viewData.ItemsData)
            {
                itemView.IsEnable = false;
            }
            
            bool allInventoryItemsExist = true;
            for (var i = 0; i < _price.Count; i++)
            {
                var item = _price.Items[i];
                var itemViewInfo = viewData.ItemsData[i];
                itemViewInfo.Icon = _spriteHolder.GetSprite( item.ItemId);
                itemViewInfo.Text = item.Count.ToString();
                itemViewInfo.IsEnable = true;
                itemViewInfo.IsWarning = _inventory.GetCount(item.ItemId) < item.Count;
                allInventoryItemsExist &= itemViewInfo.IsWarning == false;
            }
            
            //ищу условие на лимит юнитов
            var unitLimit = int.MaxValue;
            var existCount = _units.TryGetUnit(unit.UnitType, unit.Progression, out var existUnit) ? existUnit.Count : 0;
            foreach (var condition in actionConfig.Require.GetConditions())
            {
                if (condition.TypeCondition == TypeCondition.UnitsCount)
                {
                    //подумать, как убрать кастинг
                    unitLimit = ((CountConditionConfig) condition).Value;
                }
            }

            viewData.CountAndLimit = $"{existCount}/{unitLimit}";
            viewData.ButtonEnabled = _conditionProcessor.Check(actionConfig.Require) && allInventoryItemsExist;
        }
        

        public void Visit(IInventoryAction inventoryActionConfig)
        {
            inventoryActionConfig.Visit(_price);
        }
        public void Visit(IUnitAction unitActionConfig)
        {
            unitActionConfig.Visit(_unitsCollection);
        }
    }
    

    /// <summary>
    /// знает про юнитов их количество
    /// знает, можно ли купить юнита
    /// знает про список юнитов и их данные
    /// </summary>
    public class MetaTrainUnits : MonoBehaviour
    {
        private IEnumerable<MetaActionConfig> _trainActions;
        
        private List<TrainElemData> _elemsData;
        [SerializeField] private List<MetaTrainUnit> _elemsView;
        
        private IActionProcessor _actionProcessor;
        private TrainActionDataPresenter _trainDataPresenter;
        
        public void OneTimeSetup(MetaModel metaModel, IEnumerable<MetaActionConfig> trainActions, ISpriteHolderTest spriteHolder)
        {
            Assert.IsNull(_trainDataPresenter);
            
            _trainDataPresenter = new TrainActionDataPresenter(spriteHolder, metaModel.Units, metaModel.Inventory, metaModel.ConditionProcessor);
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
                _trainDataPresenter.ActionToView(trainAction, _elemsData[i]);
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
    }
}