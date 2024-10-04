using System;
using System.Collections.Generic;
using AppRen;
using Meta.Controllers;
using Meta.TestConfiguration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetaUi
{
    /// <summary>
    /// верхний бар ресурсов.
    /// олучает "модель" инвентаря и список ресурсов для показа
    /// </summary>
    public class MetaItemsBar : MonoBehaviour
    {
        private HashSet<Id> _canShowResources;
        [SerializeField] private List<MapTestId> _showList = new List<MapTestId>();
        [SerializeField] private List<ItemViewElem> _viewElems = new List<ItemViewElem>(); 

        private IInventory _inventory;
        private ISpriteHolderTest _spriteHolder;
        
        public void Setup(IInventory inventory, ISpriteHolderTest spriteHolder)
        {
            _inventory = inventory;
            _spriteHolder = spriteHolder;
            HideAllView();
            ShowEnabledViews();
            UpdateView();
        }
        

        private void HideAllView()
        {
            foreach (var viewElem in _viewElems)
            {
                viewElem.SetEnable(false);
            }
        }
        private void ShowEnabledViews()
        {
            for (var i = 0; i < _showList.Count && i < _viewElems.Count; i++)
            {
                var typeItem = _showList[i];
                var view = _viewElems[i];
                view.SetEnable(true);
                view.SetIcon(_spriteHolder.GetSprite(typeItem.Id()));
                view.SetText(_inventory.GetCount(typeItem.Id()).ToString());
            }
        }
        public void UpdateView()
        {
            for (var i = 0; i < _showList.Count && i < _viewElems.Count; i++)
            {
                var viewElem = _viewElems[i];
                var itemId = _showList[i].Id();
                
                viewElem.SetText(_inventory.GetCount(itemId).ToString());
                viewElem.SetWarning(_inventory.GetCount(itemId) >= _inventory.GetLimit(itemId));
            }
        }
    }
    
    [Serializable]
    public class ItemViewElem
    {
        public GameObject Elem;
        public TextMeshProUGUI Text;
        public Image Icon;
        public Image Warning;
        
        public ItemViewElem SetIcon(Sprite icon)
        {
            Icon.sprite = icon;
            return this;
        }

        public ItemViewElem SetText(string count)
        {
            Text.text = count;
            return this;
        }

        public ItemViewElem SetEnable(bool isEnable)
        {
            Elem.SetActive(isEnable);
            return this;
        }
        public ItemViewElem SetWarning(bool isEnable)
        {
            Warning.enabled = isEnable;
            return this;
        }
    }
}