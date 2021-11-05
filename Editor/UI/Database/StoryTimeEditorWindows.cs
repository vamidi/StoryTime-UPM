using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Extensions;
using StoryTime.Components.ScriptableObjects;

namespace StoryTime.Editor.UI
{
	using Extensions;

	public class StoryTimeEditorWindow : EditorWindow
	{
		const string k_EditorPrefValueKey = "StoryTime-ManagerTablesWindow-Selected-Tab";

		List<ToolbarToggle> m_TabToggles;
		List<EditorTab> m_TabPanels;

		private int _enumIndex;

		private VisualElement _root;
		private DrawSelected<ItemSO> drawItems;

		private bool _treeRebuild = false;

		internal int SelectedTab
		{
			get => EditorPrefs.GetInt(k_EditorPrefValueKey, 0);
			set => EditorPrefs.SetInt(k_EditorPrefValueKey, value);
		}

		//paths to SOs in project
		// private string modulePath = "Assets/Prefabs/Ships/Module/ModuleData";
		// private string colorPath = "Assets/Resources/ColorData";
		private string itemsPath = "Assets/Samples/StoryTime/1.4.2-preview/Essentials/ScriptableObjects/Item Management/Items";
		// private string recipePath = "Assets/Scripts/Industry/Recipe Data";

		[MenuItem("Tools/StoryTime/StoryTime Manager")]
		public static void OpenWindow()
		{
			GetWindow<StoryTimeEditorWindow>().Show();
		}

		private void StateChange()
		{
			_treeRebuild = true;
		}

		protected void OnEnable()
		{
			if (titleContent != null && titleContent.text == GetType().FullName)
				titleContent.text = GetType().GetNiceName().SplitPascalCase();

			TemplateContainer treeAsset = Resources.GetTemplate(nameof(StoryTimeEditorWindow));
			rootVisualElement.Add(treeAsset);

			var styles = Resources.GetStyleAsset(nameof(StoryTimeEditorWindow));
			rootVisualElement.styleSheets.Add(styles);

			_root = rootVisualElement;
			Initialize();
		}

		protected void Initialize()
		{
			// Toggle Buttons
			m_TabToggles = _root.Query<ToolbarToggle>().ToList();
			m_TabPanels = new List<EditorTab>();

			drawItems = new DrawSelected<ItemSO>();
			drawItems.SetPath(itemsPath);

			for (int i = 0; i < m_TabToggles.Count; ++i)
			{
				var toggle = m_TabToggles[i];
				var panelName = $"{toggle.name}-panel";
			    var panel = _root.Q<EditorTab>(panelName);
			    Debug.Assert(panel != null, $"Could not find panel \"{panelName}\"");
			    m_TabPanels.Add(panel);
			    panel.Editor = this;
			    panel.style.display = SelectedTab == i ? DisplayStyle.Flex : DisplayStyle.None;
			    toggle.value = SelectedTab == i;
			    int idx = i;
			    toggle.RegisterValueChangedCallback((chg) => TabSelected(idx));
			}

			Debug.Assert(m_TabPanels.Count == m_TabToggles.Count,
				"Expected the same number of tab toggle buttons and panels.");
		}

		protected void OnGUI()
		{
			if (_treeRebuild && Event.current.type == EventType.Layout)
			{
				_treeRebuild = false;
			}

			// SirenixEditorGUI.Title("The Game Manager", "Because every hobby game is overscoped", TextAlignment.Center,
				// true);
			// EditorGUILayout.Space();
		}

		private void TabSelected(int idx)
		{
			if (SelectedTab == idx)
				return;

			m_TabToggles[SelectedTab].SetValueWithoutNotify(false);
			m_TabPanels[SelectedTab].style.display = DisplayStyle.None;

			m_TabToggles[idx].SetValueWithoutNotify(true);
			m_TabPanels[idx].style.display = DisplayStyle.Flex;

			SelectedTab = idx;
		}
	}


	public class DrawSelected<T> where T : ScriptableObject
	{
		public T[] Items => items;
		public string[] Names => _names;

		public T selected;

		public string nameForNew;

		private readonly string[] _names;
		private string path;

		private T[] items;

		public DrawSelected()
		{
			// Debug.Log(typeof(T));
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

			items = new T[guids.Length];
			_names = new string[guids.Length];

			for(var i = 0; i < guids.Length; i++)
			{
				var str1 = AssetDatabase.GUIDToAssetPath(guids[i]);
				T @object = AssetDatabase.LoadAssetAtPath<T>(str1);
				items[i] = @object;

				string withoutExtension = Path.GetFileNameWithoutExtension(str1);
				string path = str1;
				path = path.Trim('/') + "/" + withoutExtension;
				EditorMenuTreeExtensions.SplitMenuPath(path, out path, out _names[i]);
				// tree.AddMenuItemAtPath(odinMenuItemSet, path, new EditorMenuItem(tree, name, @object));
			}
		}

		public void CreateNew()
		{
			if (nameForNew == "")
				return;

			T newItem = ScriptableObject.CreateInstance<T>();
			//newItem.name = "New " + typeof(T).ToString();

			if (path == "")
				path = "Assets/";

			AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
			AssetDatabase.SaveAssets();

			nameForNew = "";
		}

		public void DeleteSelected()
		{
			if(selected != null)
			{
				string _path = AssetDatabase.GetAssetPath(selected);
				AssetDatabase.DeleteAsset(_path);
				AssetDatabase.SaveAssets();
			}
		}

		public void SetSelected(object item)
		{
			var attempt = item as T;
			if (attempt != null)
				selected = attempt;
		}

		public void SetPath(string path)
		{
			this.path = path;
		}
	}
}
