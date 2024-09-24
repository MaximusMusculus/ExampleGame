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
        [SerializeField] private List<ViewElem> _viewElems = new List<ViewElem>(); 

        private IInventory _inventory;
        private ISpriteHolderTest _spriteHolder;
        
        public void Setup(IInventory inventory, ISpriteHolderTest spriteHolder)
        {
            _inventory = inventory;
            _spriteHolder = spriteHolder;
            HideAllView();
            ShowEnabledViews();
            UpdateItems();
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
                view.SetCount(_inventory.GetCount(typeItem.Id()).ToString());
            }
        }

        public void UpdateItems()
        {
            for (var i = 0; i < _showList.Count && i < _viewElems.Count; i++)
            {
                _viewElems[i].SetCount(_inventory.GetCount(_showList[i].Id()).ToString());
            }
        }
        
        
        
        [Serializable]
        private class ViewElem
        {
            public GameObject Elem;
            public TextMeshProUGUI Count;
            public Image Icon;
            public ViewElem SetIcon(Sprite icon)
            {
                Icon.sprite = icon;
                return this;
            }

            public ViewElem SetCount(string count)
            {
                Count.text = count;
                return this;
            }

            public ViewElem SetEnable(bool isEnable)
            {
                Elem.SetActive(isEnable);
                return this;
            }
        }
    }
}