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
    
    public class MetaUiRoot : MonoBehaviour, IHierarchyHandler <IUiMessage>, ISpriteHolderTest
    {
        [SerializeField]
        private List<SpriteMapElem> _spriteMap = new List<SpriteMapElem>();
        [SerializeField] private MetaTrainUnitsScreen _metaTrainUnits;

        public void Setup(MetaModel metaModel)
        {
            _metaTrainUnits.Setup(metaModel, metaModel.Config.ActionsGroups[0], this);
        }
        
        public void HandleMessage(IUiMessage uiMessage)
        {
            throw new ArgumentException("Not handled message type: " + uiMessage.GetType());
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
    }
}