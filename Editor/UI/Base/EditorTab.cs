using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Editor.Extensions;
using StoryTime.Database.ScriptableObjects;
namespace StoryTime.Editor.UI
{
	using Database;

	public class DrawListView<T> where T : TableBehaviour
	{
		public List<T> Items => _items;
		public List<string> Names => _names;
		public T Selected => _selected;

		private readonly List<string> _names;
		private readonly List<T> _items;
		private T _selected;

		public DrawListView()
		{
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

			_items = new List<T>(guids.Length);
			_names = new List<string>(guids.Length);

			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				T @object = AssetDatabase.LoadAssetAtPath<T>(path);

				_items.Add(@object);

				string withoutExtension = Path.GetFileNameWithoutExtension(path);
				path = path.Trim('/') + "/" + withoutExtension;
				EditorMenuTreeExtensions.SplitMenuPath(path, out path, out var itemName);
				_names.Add(itemName);
				// tree.AddMenuItemAtPath(odinMenuItemSet, path, new EditorMenuItem(tree, name, @object));
			}
		}

		public void SetSelected(T item)
		{
			var attempt = item;
			if (attempt != null)
				_selected = attempt;
		}

		public bool DeleteSelected()
		{
			if(_selected != null &&
			   EditorUtility.DisplayDialog("Deleting item",
				   "Are you sure you want to delete this item?", "Delete", "Cancel"))
			{
				string path = AssetDatabase.GetAssetPath(_selected);
				AssetDatabase.DeleteAsset(path);
				AssetDatabase.SaveAssets();
				return true;
			}

			return false;
		}

		/*
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
		*/
	}

	public abstract class EditorTab : VisualElement
	{
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public EditorWindow Editor { protected get; set; }

		protected bool IsJsonObj;
		protected SerializedObject serializedObject;
		protected string defaultPath = "Assets/";
		protected UnityEditor.Editor editor;

		public abstract void Repaint();
	}

	public abstract class EditorTab<T> : EditorTab where T : TableBehaviour
	{
		protected readonly DrawListView<T> ItemListView;

		protected Dictionary<uint, string> PopulatedList = new()
		{
			{ UInt32.MaxValue, "None" }
		};

		protected int choiceIndex = Int32.MaxValue;

		private readonly IMGUIContainer _defaultInspector;
		private ListView _listView;

		protected EditorTab()
		{
			ItemListView = new DrawListView<T>();

			DatabaseSyncModule.onFetchCompleted += (_, tableName) =>
			{
				T target = ItemListView.Selected;
				if (target != null && target.Name == tableName)
				{
					Debug.Log($"Re-generating list of {tableName}");
					GenerateList();
				}
			};

			_defaultInspector = new IMGUIContainer(() =>
			{
				if(editor && editor.target)
					editor.OnInspectorGUI();
			});
		}

		public override void Repaint()
		{
			var button = this.Q<Button>("btn-new");
			if (button != null)
			{
				button.clickable = new Clickable(CreateNew);
			}

			button = this.Q<Button>("btn-select");
			if (button != null)
			{
				button.clickable = new Clickable(SelectItem);
			}

			button = this.Q<Button>("btn-delete");
			if (button != null)
			{
				button.clickable = new Clickable(DeleteItem);
			}

			// TODO make template
			_listView = RetrieveListView();
			_listView.Clear();
			_listView.makeItem = () => new Label();
			_listView.bindItem = (element, i) =>
			{
				if (element is Label el)
				{
					el.text = ItemListView.Names[i];
					el.style.fontSize = 14;
					el.style.borderBottomWidth = new StyleFloat(1);
					var padding = new StyleLength(12);
					el.style.paddingLeft = padding;
					el.style.paddingRight = padding;
					var c = 24f / 255f;
					el.style.borderBottomColor = new StyleColor(new Color(c, c,  c, 0.5f));
				}
			};

			_listView.itemsSource = ItemListView.Items;
			_listView.fixedItemHeight = 30;
			_listView.selectionType = SelectionType.Single;

			Box cardInfo = this.Query<Box>("item-info").First();
			cardInfo.Clear();

			cardInfo.Add(_defaultInspector);

			_listView.onSelectionChange += SelectChange;
			_listView.Rebuild();
		}

		protected ListView RetrieveListView()
		{
			var listView = this.Q<ListView>("item-list");
			if (listView == null)
			{
				throw new Exception($"Could find list view for {GetType()}");

			}

			return listView;
		}

		protected abstract void DrawSelection(Box cardInfo, T selection);

		protected virtual void OnChanged() { }

		protected virtual void SelectChange(IEnumerable<object> enumerable)
		{
			foreach (var obj in enumerable)
			{
				T it = obj as T;

				if (it != null)
				{
					ItemListView.SetSelected(it);
					serializedObject = new SerializedObject(it);

					GenerateList();

					// DrawSelection(cardInfo, it);

					// If there isn't a Transform currently selected then destroy the existing editor
					if (editor != null)
						UnityEngine.Object.DestroyImmediate(editor);

					editor = UnityEditor.Editor.CreateEditor(it);

					// Selection.activeObject = it;
				}
			}
		}

		protected void LoadPreviewImage(Texture texture)
		{
			if (texture == null)
				return;

			var previewImage = this.Query<Image>("preview").First();
			previewImage.image = texture;
		}

		private void GenerateList()
		{
			T target = ItemListView.Selected;
			if (target != null)
			{
				choiceIndex = Array.FindIndex(PopulatedList.Keys.ToArray(), idx => idx == target.ID);
				// retrieve the column we need to show
				BaseEditorList.GenerateList(ref PopulatedList, target, out IsJsonObj);
			}
		}

		private void SelectItem()
		{
			if(serializedObject.targetObject != null)
				Selection.activeObject = serializedObject.targetObject;
		}

		private void CreateNew()
		{
			var path = EditorUtility.SaveFilePanel("Create new", defaultPath, "", "asset");
			if (string.IsNullOrEmpty(path))
				return;

			path = path.Substring(path.ToLower().IndexOf("assets/", StringComparison.Ordinal));
			T newItem = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(newItem, path);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();

			defaultPath = path;

			string withoutExtension = Path.GetFileNameWithoutExtension(defaultPath);
			path = path.Trim('/') + "/" + withoutExtension;
			EditorMenuTreeExtensions.SplitMenuPath(path, out path, out var itemName);

			ItemListView.Items.Add(newItem);
			ItemListView.Names.Add(itemName);

			// TODO sort
			// ItemListView.Items.Sort((x,y) => String.Compare(x.Name, y.Name));

			_listView.Rebuild();
		}

		private void DeleteItem()
		{
			if (ItemListView.Selected)
			{
				int idx = ItemListView.Items.FindIndex((m) => m == ItemListView.Selected);
				ItemListView.Items.RemoveAt(idx);
				ItemListView.Names.RemoveAt(idx);

				if (ItemListView.DeleteSelected())
				{
					// TODO sort
					// ItemListView.Items.Sort((x,y) => String.Compare(x.Name, y.Name));

					_listView.Rebuild();
				}

				return;
			}

			Debug.LogWarning("No item selected!");
		}
	}
}
