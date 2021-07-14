using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	using Database;
	using Extensions;

	public class DrawListView<T> where T : Components.TableBehaviour
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

			for(var i = 0; i < guids.Length; i++)
			{
				var str1 = AssetDatabase.GUIDToAssetPath(guids[i]);
				T @object = AssetDatabase.LoadAssetAtPath<T>(str1);

				_items.Add(@object);

				string withoutExtension = Path.GetFileNameWithoutExtension(str1);
				string path = str1;
				path = path.Trim('/') + "/" + withoutExtension;
				EditorMenuTreeExtensions.SplitMenuPath(path, out path, out var itemName);
				_names.Add(itemName);
				// tree.AddMenuItemAtPath(odinMenuItemSet, path, new EditorMenuItem(tree, name, @object));
			}
		}

		public virtual void SetSelected(T item)
		{
			var attempt = item;
			if (attempt != null)
				_selected = attempt;
		}

		public virtual bool DeleteSelected()
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
		public EditorWindow Editor { get; set; }

		protected bool IsJsonObj;
		protected SerializedObject serializedObject;
		protected string defaultPath = "Assets/";
		protected UnityEditor.Editor _editor;
	}

	public abstract class EditorTab<T> : EditorTab where T : Components.TableBehaviour
	{
		protected readonly DrawListView<T> ItemListView;

		protected Dictionary<uint, string> m_PopulatedList = new Dictionary<uint, string>();

		protected int _choiceIndex = Int32.MaxValue;

		private IMGUIContainer m_DefaultInspector = null;
		private ListView m_ListView = null;

		protected EditorTab()
		{
			ItemListView = new DrawListView<T>();

			DatabaseSyncModule.onFetchCompleted += (o, args) =>
			{
				TableDatabase.Get.Refresh();
				GenerateList();
			};

			m_DefaultInspector = new IMGUIContainer(() =>
			{
				if(_editor && _editor.target)
					_editor.OnInspectorGUI();
			});
		}

		protected void Initialize(ListView listView)
		{
			var button = this.Q<Button>("btn-new");
			if (button != null)
			{
				button.clickable = new Clickable(CreateNew);
			}

			button = this.Q<Button>("btn-delete");
			if (button != null)
			{
				button.clickable = new Clickable(DeleteItem);
			}

			// TODO make template
			m_ListView = listView;
			m_ListView.makeItem = () => new Label();
			m_ListView.bindItem = (element, i) =>
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

			m_ListView.itemsSource = ItemListView.Items;
			m_ListView.itemHeight = 30;
			m_ListView.selectionType = SelectionType.Single;

			Box cardInfo = this.Query<Box>("item-info").First();
			cardInfo.Clear();

			cardInfo.Add(m_DefaultInspector);

			m_ListView.onSelectionChange += enumerable => SelectChange(cardInfo, enumerable);
			m_ListView.Refresh();
		}

		protected abstract void DrawSelection(Box cardInfo, T selection);

		protected virtual void OnChanged() { }

		protected virtual void SelectChange(Box cardInfo, IEnumerable<object> enumerable)
		{
			foreach (var obj in enumerable)
			{
				T it = obj as T;

				if (it != null)
				{
					ItemListView.SetSelected(it);
					serializedObject = new SerializedObject(it);

					GenerateList();

					DrawSelection(cardInfo, it);
					_editor = UnityEditor.Editor.CreateEditor(it);

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

		protected void GenerateList()
		{
			T target = ItemListView.Selected;
			if (target != null)
			{
				_choiceIndex = Array.FindIndex(m_PopulatedList.Keys.ToArray(), idx => idx == target.ID);

				var binary = TableDatabase.Get.GetBinary(target.Name);
				string linkColumn = target.LinkedColumn;
				uint linkId = target.LinkedID;
				bool linkTable = target.LinkedTable != String.Empty;

				// retrieve the column we need to show
				m_PopulatedList = linkColumn != "" && (linkTable || linkId != UInt32.MaxValue) ? binary.PopulateWithLink(
					target.DropdownColumn,
					linkColumn,
					linkId,
					out IsJsonObj,
					target.LinkedTable
				) : binary.Populate(target.DropdownColumn, out IsJsonObj);
			}
		}

		private void CreateNew()
		{
			var path = EditorUtility.SaveFilePanel("Create Table Collection", defaultPath, "", "asset");
			if (string.IsNullOrEmpty(path))
				return;

			path = path.Substring(path.ToLower().IndexOf("assets/"));
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

			m_ListView.Refresh();
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

					m_ListView.Refresh();
				}

				return;
			}

			Debug.LogWarning("No item selected!");
		}
	}
}
