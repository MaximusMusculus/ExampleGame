using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Meta.ConfigOdin
{
    public class ElemDrawer<TElem> : OdinValueDrawer<TElem> where TElem : ConfigElem
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if (label != null)
            {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            }
            else
            {
                rect = EditorGUI.IndentedRect(rect);
            }

            ConfigElem item = ValueEntry.SmartValue;
            Texture texture = null;

            if (item)
            {
                texture = GUIHelper.GetAssetThumbnail(item.Icon, typeof(ConfigElem), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : item.Name);
            }

            ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), item, texture, ValueEntry.BaseValueType);
        }
    }
}