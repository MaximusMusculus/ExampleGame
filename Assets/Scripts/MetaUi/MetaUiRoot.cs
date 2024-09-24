using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta;
using Meta.TestConfiguration;
using UnityEngine;

namespace MetaUi
{
    public interface ISpriteHolderTest
    {
        Sprite GetSprite(Id typeEntity);
    }
    
    public class MetaUiRoot : MonoBehaviour, IHierarchyHandler <UiEventSwitchScreen>, ISpriteHolderTest
    {
        [SerializeField]
        private List<SpriteMapElem> _spriteMap = new List<SpriteMapElem>();
        [SerializeField] private MetaTrainUnitsScreen _metaTrainUnits;
        [SerializeField] private MetaPlayScreen _metaPlayScreen;

        public void Setup(MetaModel metaModel)
        {
            _metaPlayScreen.Setup(metaModel, this);
            _metaTrainUnits.Setup(metaModel, metaModel.Config.ActionsGroups[0], this);
            
            //переключение скринов.
            //при переключении(перед переключением) - активация с обновлением.
        }
        
        [Serializable]
        private class SpriteMapElem
        {
            public MapTestId TypeEntity;
            public Sprite Icon;
        }
        public Sprite GetSprite(Id typeEntity)
        {
            return _spriteMap.FirstOrDefault(s => typeEntity.Equals((ushort) s.TypeEntity))?.Icon;
        }
        
        public void HandleMessage(UiEventSwitchScreen message)
        {
            switch (message.ScreenType)
            {
                case MetaScreenType.Units:
                    _metaTrainUnits.UpdateView();
                    _metaPlayScreen.gameObject.SetActive(false);
                    _metaTrainUnits.gameObject.SetActive(true);
                    break;
                case MetaScreenType.Play:
                    _metaPlayScreen.UpdateView();
                    _metaTrainUnits.gameObject.SetActive(false);
                    _metaPlayScreen.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}