using System.Collections.Generic;
using System.Linq;
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
        private MetaConfigOdin _metaConfig;
        private OdinMenuTree _tree;
        
        public static void Open(MetaConfigOdin configOdin)
        {
            var window = GetWindow<MetaOdinConfigInspector>();
            window._metaConfig = configOdin;
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        
        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree(true);
            _tree.DefaultMenuStyle.IconSize = 30.00f;
            _tree.Config.DrawSearchToolbar = true;
            
            _tree.Add("Resources/", new GroupElemCreator(_metaConfig.Resources, this));
            foreach (var resource in _metaConfig.Resources)
            {
                _tree.Add("Resources/" + resource.name, resource);
            }

            foreach (var unit in _metaConfig.Units)
            {
                _tree.Add("Units/" + unit.name, unit);
            }

            foreach (var quest in _metaConfig.Quests)
            {
                _tree.Add("Quests/" + quest.name, quest);
            }
            
            
            // Добавьте к предметам маркеры перетаскивания, чтобы их можно было легко перетаскивать в инвентарь, если персонажи и т. д.
            _tree.EnumerateTree().Where(x => x.Value as ResourceConfigOdin).ForEach(AddDragHandles);
            _tree.EnumerateTree().Where(x => x.Value as UnitConfigOdin).ForEach(AddDragHandles);
            
            _tree.EnumerateTree().AddIcons<ResourceConfigOdin>(x => x.Icon);
            _tree.EnumerateTree().AddIcons<UnitConfigOdin>(x => x.Icon);
            
            return _tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
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
        private readonly IMenuTree _menuTree;

        public GroupElemCreator(List<ResourceConfigOdin> resourceConfig, IMenuTree menuTree)
        {
            _resourceConfig = resourceConfig;
            _menuTree = menuTree;
        }
        
        [Button]
        public void CreateNew()
        {
            _menuTree.UpdateThree();
        }
        
        

    }
}