using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Meta.ConfigOdin
{
	public class ConfigLinkAttribute : Attribute
	{
		public Type Type { get; } //filter by type

		public ConfigLinkAttribute(Type type)
		{
			Type = type;
		}
	}

	//[ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]

	/*public class DefinitionLinkAttributeDrawer : OdinAttributeDrawer<ConfigLinkAttribute, ulong>
	{
		protected override void DrawPropertyLayout(GUIContent label)
		{
			var dropDownProvider = MetaOdinConfigInspector.DropDownElemProvider;

			//тут логика отбора дефоф по типу и построения выпадашек
			//DefinitionExtension.DefinitonHolder;
			var popupElems = GetPopupElems(dropDownProvider);

			if (Attribute.componentType != null)
			{
				var componented = definitionHolder.GetDefinitions(typeof(DefinitionComponented)).ToList();
				var popupElemsComponented = GetPopupElemsComponented(componented, Attribute.componentType);
				foreach (var elem in popupElemsComponented)
				{
					popupElems.Add(elem.Key, elem.Value);
				}
			}
			


			var keyArray = popupElems.Select(c => c.Key).ToList();
			var index = keyArray.FindIndex(c => c == ValueEntry.SmartValue);
			var popUp = popupElems.Select(c => c.Value).ToArray();
			index = EditorGUILayout.Popup(label.text, index, popUp, EditorStyles.popup);
			ValueEntry.SmartValue = index > -1 ? keyArray[index] : "Empty";

			ValueEntry.ApplyChanges();
		}
	}*/
}
