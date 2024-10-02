using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta;
using Meta.TestConfiguration;
using UnityEngine;
using UnityEngine.Assertions;

namespace MetaUi
{
    public interface ISpriteHolderTest
    {
        Sprite GetSprite(Id typeEntity);
    }
    
    public enum MetaScreenType
    {
        None,
        Units,
        Play
    }

    public class MetaUiRoot : MonoBehaviour, IHierarchyHandler <UiEventSwitchScreen>, IHierarchyHandler<UiEventRunAction>, ISpriteHolderTest
    {
        [SerializeField] private List<SpriteMapElem> _spriteMap = new List<SpriteMapElem>();
        [SerializeField] private MetaTrainUnitsScreen _metaTrainUnits;
        [SerializeField] private MetaPlayScreen _metaPlayScreen;

        private ITestInputCmdExecutor _inputCmdExecutor;
        private Dictionary<Id, Sprite> _spritesHash;

        public void Setup(MetaModel metaModel, ITestInputCmdExecutor inputCmdExecutor)
        {
            Assert.IsNull(_spritesHash);

            _spritesHash = new Dictionary<Id, Sprite>(_spriteMap.Count);
            foreach (var spriteMapElem in _spriteMap)
            {
                _spritesHash[spriteMapElem.TypeEntity.Id()] = spriteMapElem.Icon;
            }
            
            _inputCmdExecutor = inputCmdExecutor;
            _metaPlayScreen.Setup(metaModel, this);
            _metaTrainUnits.Setup(metaModel, metaModel.Config.ActionsGroups[0], this);
        }
        
        public void HandleEvent(UiEventRunAction message)
        {
            _inputCmdExecutor.Execute(message);
        }
        
        public void HandleEvent(UiEventSwitchScreen message)
        {
            //перед переключением - активация с обновлением.
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



        //----
        [Serializable]
        private class SpriteMapElem
        {
            public MapTestId TypeEntity;
            public Sprite Icon;
        }
        public Sprite GetSprite(Id typeEntity)
        {
            _spritesHash.TryGetValue(typeEntity, out var sprite);
            return sprite;
        }
    }
}