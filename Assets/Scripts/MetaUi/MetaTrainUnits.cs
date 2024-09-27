using System.Collections.Generic;
using System.Linq;
using Meta;
using Meta.Configs;
using Meta.Configs.Conditions;
using Meta.Controllers;
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

        public List<ItemViewData> ItemsData = new List<ItemViewData>() {new ItemViewData(), new ItemViewData()};
        
        public class ItemViewData
        {
            public bool IsEnable;
            public Sprite Icon;
            public string Text;
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
        private ISpriteHolderTest _spriteHolder;
        
        public void Setup(MetaModel metaModel, IEnumerable<MetaActionConfig> trainActions, ISpriteHolderTest spriteHolder)
        {
            _unitsController = metaModel.Units;
            _conditions = metaModel.ConditionProcessor;
            _trainActions = trainActions;
            _spriteHolder = spriteHolder;
            BindDataToView();
            UpdateUnits();
        }
        
        
        public void UpdateUnits()
        {
            int i = 0;
            foreach (var trainAction in SelectTrainActions())
            {
                if (i > _elemsData.Count-1)
                {
                    break;
                }
                
                FillTrainElemView(trainAction, _elemsData[i]);
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
        
        private IEnumerable<MetaActionConfig> SelectTrainActions()
        {
            //отбираем только те действия, которые добавляют юнитов
            //если будет такое часто - передать фильтр
            foreach (var trainAction in _trainActions)
            {
                foreach (var action in trainAction.Actions.GetAll())
                {
                    if (action.TypeMetaAction == TypeMetaAction.UnitAdd)
                    {
                        yield return trainAction;
                    }
                }
            }
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
        
        private void FillTrainElemView(MetaActionConfig actionConfig, TrainElemData elemData)
        {
            elemData.ActionConfig = actionConfig;
            //первая, мучительная процедура распаковки)) экшена.  Далее - либо, буду делать типизированные (классов что ли жалко)
            //либо спец адаптеры.
            
            //если захочется тутор, то кондишен оборачивается доп-но в тутор логику
            elemData.ButtonEnabled = _conditions.Check(actionConfig.Require);
            
            // я знаю, что в списке действий - должно быть действие по добавлению юнита
            var addUnitAction = actionConfig.Actions.Untis.FirstOrDefault(s => s.TypeMetaAction == TypeMetaAction.UnitAdd);
            Assert.IsNotNull(addUnitAction);

            elemData.Title = "UnitType: " + addUnitAction.TypeUnit;
            elemData.Description = "Description: ";
            elemData.Icon = _spriteHolder.GetSprite(addUnitAction.TypeUnit);

            var unitCount = 0;
            var unitLimit = int.MaxValue;
            
            if(_unitsController.TryGetUnit(addUnitAction.TypeUnit, addUnitAction.Progression, out var unit))
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
            elemData.CountAndLimit = $"{unitCount}/{unitLimit}";
        }


    }
}