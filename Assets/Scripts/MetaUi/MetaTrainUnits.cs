using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Configs.Conditions;
using Meta.Controllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace MetaUi
{
    public class TrainElemData : IUiEvent
    {
        public string Title;
        public string Description;
        public string CountAndLimit;
        
        public bool ButtonEnabled;
        public Sprite Icon;

        public Id UnitType;
        public IUiEventHandler ParentEventHandler;
        public TrainUiEvent Event = new TrainUiEvent();
    }
    public class TrainUiEvent : IUiEvent
    {
        public Id UnitType;
        public Transform UnitPivot;
    }

    
    /// <summary>
    /// знает про юнитов их количество
    /// знает, можно ли купить юнита
    /// знает про список юнитов и их данные
    /// </summary>
    public class MetaTrainUnits : MonoBehaviour, IUiEventHandler, IUiCustomEventHandler<TrainUiEvent>
    {
        private IUnitsController _unitsController;
        private IConditionProcessor _conditions;
        private IEnumerable<MetaActionConfig> _trainActions;
        private IUiEventHandler _parentUiEventHandler; //??

        private List<TrainElemData> _elemsData;
        [SerializeField] private List<MetaTrainUnit> _elemsView;

        public void Setup(IUnitsController unitsController, IConditionProcessor conditions, IEnumerable<MetaActionConfig> trainActions,
            IUiEventHandler parentUiEventHandler) //это в базовый класс
        {
            _unitsController = unitsController;
            _conditions = conditions;
            _trainActions = trainActions;
            _parentUiEventHandler = parentUiEventHandler;
            FillUnits();
        }


        // тут как то делается обработка событий. 
        // суть в том, что я тут могу объявить типизированный обработчик
        //и по идее должен вывалиться в обработку сюда. Тут - я либо прерываю цепочку, либо пускаю эвент дальше.
        //если я не реализую никакой интерфейс прослушивателя - то обработка событий идет дальше наверх
        public void HandleEvent(IUiEvent uiEvent)
        {
            //передать выше или обработать самому?
            _parentUiEventHandler.HandleEvent(uiEvent);
        }
        public void Handle(TrainUiEvent uiEvent)
        {
            _parentUiEventHandler.HandleEvent(uiEvent);
        }


        private void FillUnits()
        {
            BindDataToView();
            int i = 0;
            foreach (var trainAction in SelectTrainActions())
            {
                FillTrainElemView(trainAction, _elemsData[i]);
                i++;
            }

            for (var j = 0; j < _elemsView.Count; j++)
            {
                if (j < i) _elemsView[j].RefreshElem();
                else _elemsView[j].gameObject.SetActive(false);
            }
        }

        private IEnumerable<MetaActionConfig> SelectTrainActions()
        {
            //отбираем только те действия, которые добавляют юнитов
            //если будет такое часто - передать фильтр
            foreach (var trainAction in _trainActions)
            {
                foreach (var action in trainAction.Actions)
                {
                    if (action.TypeAction == TypeAction.UnitAdd)
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
                var viewData = new TrainElemData {ParentEventHandler = this};
                _elemsData.Add(viewData);
                trainElem.SetData(viewData);
            }
        }


        //первая, мучительная процедура распаковки)) экшена.  Далее - либо, буду делать типизированные (классов что ли жалко)
        //либо спец адаптеры.
        private void FillTrainElemView(MetaActionConfig actionConfig, TrainElemData elemData)
        {
            elemData.ButtonEnabled = _conditions.Check(actionConfig.Require); //если захочется тутор, то кондишен оборачивается доп-но в тутор логику
            // я знаю, что в списке действий - должно быть действие по добавлению юнита
            var addUnitAction = actionConfig.Actions.Untis.FirstOrDefault(s => s.TypeAction == TypeAction.UnitAdd);
            Assert.IsNotNull(addUnitAction);

            elemData.Title = "UnitType: " + addUnitAction.TypeUnit;
            elemData.Description = "Description: ";

            var unitCount = 0;
            var unitLimit = int.MaxValue;
            
            if(_unitsController.TryGetUnit(addUnitAction.TypeUnit, addUnitAction.Progression, out var unit))
            {
                unitCount = unit.Count;
            }
            
            foreach (var actionsItem in actionConfig.Actions.Items)
            {
                elemData.Description += $"{actionsItem.TypeItem}:{actionsItem.Count}, ";
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