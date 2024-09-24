using System.Collections.Generic;
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

        [SerializeField] private List<ItemViewElem> _priceView;
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
            FillPrice();
        }
        private void FillPrice()
        {
            for (var i = 0; i < _priceView.Count; i++)
            {
                if (_data.ItemsData.Count > i)
                {
                    var elemData = _data.ItemsData[i];
                    _priceView[i].SetEnable(elemData.IsEnable).SetIcon(elemData.Icon).SetText(elemData.Text);
                }
                else
                {
                    _priceView[i].SetEnable(false);
                }
            }
        }
        
        public void OnTrainClick()
        {
            this.SendHierarchy(new UiEventTrainUnit(_data.ActionConfig, _iconImage.transform));
        }
    }
}