using System.Collections.Generic;
using AppRen;
using Meta.Controllers;
using UnityEngine;

namespace MetaUi
{
    /// <summary>
    /// верхний бар ресурсов.
    /// олучает "модель" инвентаря и список ресурсов для показа
    /// </summary>
    public class MetaItemsBar : MonoBehaviour
    {
        private HashSet<Id> _canShowResources;
        public void Setup(IInventoryController inventory, HashSet<Id> canShowResources)
        {
            _canShowResources = canShowResources;
        }
    }
}