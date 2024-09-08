using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Meta.ConfigOdin
{
    /// <summary>
    /// Открытия и работа с существующим конфигом
    /// </summary>
    public class MetaOdinConfigInspector : OdinMenuEditorWindow, IMenuTree
    {
        public static IDropDownElemProvider DropDownElemProvider { get; private set; }
        
        private MetaConfigOdin _metaConfig;
        private IIdProvider _idProvider;
        private OdinMenuTree _tree;
        
        public static void Open(MetaConfigOdin configOdin)
        {
            var window = GetWindow<MetaOdinConfigInspector>();
            Debug.Log("open");
            
            window._metaConfig = configOdin;
            window.ReInitialize();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        private void ReInitialize()
        {
            _idProvider = new IdProvider(_metaConfig.IdProviderData);
            DropDownElemProvider = new MetaDropDownElemProvider(_metaConfig.IdProviderData, _metaConfig);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            if (_metaConfig != null && _idProvider == null)
            {
                ReInitialize();
            }
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree(true);
            _tree.Config.DrawSearchToolbar = true;
            
            _tree.Add("Resources/", new GroupElemCreator(_idProvider, _metaConfig.Resources, this));
            foreach (var resource in _metaConfig.Resources)
            {
                _tree.Add("Resources/" + resource.TechName, resource);
                //link
                //Resource.SetDropDownProvider(this);
                //
            }

            foreach (var quest in _metaConfig.Quests)
            {
                _tree.Add("Quests/" + quest.TechName, quest);
            }
            
            return _tree;
        }



        public void UpdateThree()
        {
            ForceMenuTreeRebuild();
        }
    }

    public interface IMenuTree
    {
        void UpdateThree();
    }
    

    public class GroupElemCreator
    {
        private readonly List<ResourceConfigOdin> _resourceConfig;
        private readonly IIdProvider _idProvider;
        private readonly IMenuTree _menuTree;

        public GroupElemCreator(IIdProvider idProvider, List<ResourceConfigOdin> resourceConfig, IMenuTree menuTree)
        {
            _idProvider = idProvider;
            _resourceConfig = resourceConfig;
            _menuTree = menuTree;
        }
        
        [Button]
        public void CreateNew()
        {
            var id = _idProvider.GetNext();
            _resourceConfig.Add(new ResourceConfigOdin {Id = id, TechName = "Resource" + id}); //create instance
            _menuTree.UpdateThree();
        }
        
        

    }
}