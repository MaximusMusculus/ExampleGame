using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace MetaUi
{
    public class MetaTrainUnit : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _countAndLimitText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _trainButton;

        private TrainElemData _data;
        
        public void SetData(TrainElemData data)
        {
            _data = data;
        }

        public void RefreshElem()
        {
            Assert.IsNotNull(_data);
            _titleText.text = _data.Title;
            _iconImage.sprite = _data.Icon;
            _descriptionText.text = _data.Description;
            _countAndLimitText.text = _data.CountAndLimit;
            _trainButton.interactable = _data.ButtonEnabled;
        }
        
        public void OnTrainClick()
        {
            Debug.Log("OnTrainClick");
            var trainUiEvent = _data.Event;
            trainUiEvent.UnitType = _data.UnitType;
            trainUiEvent.UnitPivot = _iconImage.transform;
            _data.ParentEventHandler.HandleEvent(trainUiEvent);
        }

    }
}