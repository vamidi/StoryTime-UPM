using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;

namespace Domains.Odin.Editor
{
	public class DataManager: OdinMenuEditorWindow
	{
		private static readonly Type[] TypesToDisplay = TypeCache.GetTypesWithAttribute<ManageableDataAttribute>()
			.OrderBy(m => m.Name)
			.ToArray();

		private Type _selectedType;

		[MenuItem("Tools/Data Manager")]
		private static void OpenEditor() => GetWindow<DataManager>();

		protected override void OnImGUI()
		{
			//draw menu tree for SOs and other assets
			if (GUIUtils.SelectButtonList(ref _selectedType, TypesToDisplay))
			{
				ForceMenuTreeRebuild();
			}

			base.OnImGUI();
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			if (_selectedType != null)
			{
				Debug.Log(TypesToDisplay.Length);
				tree.AddAllAssetsAtPath(_selectedType.Name, "Assets/", _selectedType, true, true);
			}

			return tree;
		}
	}
}
#endif