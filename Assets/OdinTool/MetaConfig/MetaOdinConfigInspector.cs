using System;
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
    /// OdinTool/MetaConfig/Config/
    /// </summary>
    public class MetaConfigInspector : OdinMenuEditorWindow, IMenuTree
    {
        private MetaConfigOdin _metaConfig;
        private OdinMenuTree _tree;

        public static void Open(MetaConfigOdin configOdin)
        {
            var window = GetWindow<MetaConfigInspector>();
            window._metaConfig = configOdin;
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree(true);
            _tree.DefaultMenuStyle.IconSize = 30.00f;
            _tree.Config.DrawSearchToolbar = true;

            _tree.Add("Resources/", new ElemCreator<ResourceConfigOdin>(this, x => _metaConfig.Resources.Add(x)));
            foreach (var resource in _metaConfig.Resources)
            {
                _tree.Add("Resources/" + resource.name, resource);
            }

            _tree.Add("Units/", new ElemCreator<UnitConfigOdin>(this, x => _metaConfig.Units.Add(x)));
            foreach (var unit in _metaConfig.Units)
            {
                _tree.Add("Units/" + unit.name, unit);
            }

            _tree.Add("Quests/", new ElemCreator<QuestConfigOdin>(this, x => _metaConfig.Quests.Add(x)));
            foreach (var quest in _metaConfig.Quests)
            {
                _tree.Add("Quests/" + quest.name, quest);
            }


            // Добавьте к предметам маркеры перетаскивания, чтобы их можно было легко перетаскивать в инвентарь, если персонажи и т. д.
            _tree.EnumerateTree().Where(x => x.Value as ResourceConfigOdin).ForEach(AddDragHandles);
            _tree.EnumerateTree().Where(x => x.Value as UnitConfigOdin).ForEach(AddDragHandles);
            _tree.EnumerateTree().Where(x => x.Value as QuestConfigOdin).ForEach(AddDragHandles);

            _tree.EnumerateTree().AddIcons<ResourceConfigOdin>(x => x.Icon);
            _tree.EnumerateTree().AddIcons<UnitConfigOdin>(x => x.Icon);
            _tree.EnumerateTree().AddIcons<QuestConfigOdin>(x => x.Icon);

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

        public void TrySelectMenuItemWithObject(ScriptableObject target)
        {
            base.TrySelectMenuItemWithObject(target);
        }
    }

    public interface IMenuTree
    {
        void UpdateThree();
        void TrySelectMenuItemWithObject(ScriptableObject target);
    }

    public class ElemCreator<T> where T : ConfigElem
    {
        private readonly IMenuTree _menuTree;
        private readonly Action<T> _addToConfig;

        public ElemCreator(IMenuTree menuTree, Action<T> addToConfig)
        {
            _menuTree = menuTree;
            _addToConfig = addToConfig;
        }

        [Button]
        public void CreateNew()
        {
            ScriptableObjectCreator.ShowDialog<T>("Assets/OdinTool/MetaConfig/Config/", obj =>
            {
                obj.Name = obj.name;
                _addToConfig(obj);
                _menuTree.UpdateThree();
                _menuTree.TrySelectMenuItemWithObject(obj);
            });
        }
    }




}