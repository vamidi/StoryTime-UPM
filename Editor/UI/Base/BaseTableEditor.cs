using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Components.ScriptableObjects;

namespace StoryTime.Editor
{
	using Database;

	public class BaseTableEditor<T> : UnityEditor.Editor where T : TableBehaviour
	{
		protected int Choice
		{
			set => _choiceIndex = value;
		}

		protected bool IsJsonObj;

		private Dictionary<uint, string> m_PopulatedList = new Dictionary<uint, string>();
		private int _choiceIndex;

		public override VisualElement CreateInspectorGUI()
		{
			return base.CreateInspectorGUI();
		}

		public virtual void OnEnable()
		{
			DatabaseSyncModule.onFetchCompleted += (o, args) =>
			{
				TableDatabase.Get.Refresh();
				GenerateList();
			};
			GenerateList();

			// set the id right
			var t = target as TableBehaviour;

			if (t) _choiceIndex = Array.FindIndex(m_PopulatedList.Keys.ToArray(), idx => idx == t.ID);
		}

		protected virtual string GetGameObjectName() => typeof(T).Name;

		protected T GetObject()
		{
			if (target is T) {
				return (T)target;
			}
			try {
				return (T)Convert.ChangeType(target, typeof(T));
			}
			catch (InvalidCastException) {
				return default;
			}
		}

		protected void RenameAsset(UnityEngine.Object t, string fileName)
		{
			string assetPath = UnityEditor.AssetDatabase.GetAssetPath(t.GetInstanceID());
			UnityEditor.AssetDatabase.RenameAsset(assetPath, fileName);
			UnityEditor.AssetDatabase.SaveAssets();
		}

		protected virtual void OnChanged() { }

		public override void OnInspectorGUI()
		{
			GUIContent arrayLabel = new GUIContent("ID");
			_choiceIndex = UnityEditor.EditorGUILayout.Popup(arrayLabel, _choiceIndex, m_PopulatedList.Values.ToArray());

			// Draw the default inspector
			base.OnInspectorGUI();
			//
			// DrawDefaultInspector();

			var t = target as TableBehaviour;
			if (_choiceIndex >= 0)
			{
				var newID = m_PopulatedList.Keys.ToArray()[_choiceIndex];
				if (t && t.ID != newID)
				{
					// TODO split these two arrays
					// Update the selected choice in the underlying object
					t.ID = newID;

					OnChanged();

					// Save the changes back to the object
					UnityEditor.EditorUtility.SetDirty(target);
				}
			}
		}

		protected virtual void GenerateList()
		{
			var tblComp = target as TableBehaviour;
			if (tblComp != null)
			{
				_choiceIndex = Array.FindIndex(m_PopulatedList.Keys.ToArray(), idx => idx == tblComp.ID);
				var binary = TableDatabase.Get.GetBinary(tblComp.Name);
				string linkColumn = tblComp.LinkedColumn;
				uint linkId = tblComp.LinkedID;
				bool linkTable = tblComp.LinkedTable != String.Empty;

				// retrieve the column we need to show
				m_PopulatedList = linkColumn != "" && (linkTable || linkId != UInt32.MaxValue) ? binary.PopulateWithLink(
					tblComp.DropdownColumn,
					linkColumn,
					linkId,
					out IsJsonObj,
					tblComp.LinkedTable
				) : binary.Populate(tblComp.DropdownColumn, out IsJsonObj);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(TableBehaviour), true)]
	public class TableEditor : BaseTableEditor<TableBehaviour>
	{
		public override void OnEnable()
		{
			base.OnEnable();
			GenerateList();
		}
	}

	/// <summary>
	/// ActorSO editor settings
	/// </summary>
	[UnityEditor.CustomEditor(typeof(CharacterSO))]
	public class ActorEditor : BaseTableEditor<CharacterSO>
	{
		public override void OnEnable()
		{
			base.OnEnable();
			GenerateList();
		}

		protected override void OnChanged()
		{
			var t = target as CharacterSO;
			if (t && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);
				// set all the values from the selected row
				if (row != null) CharacterSO.CharacterTable.ConvertRow(row, t);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(StorySO))]
	public class StoryEditor : BaseTableEditor<StorySO>
	{
		protected override void OnChanged()
		{
			var t = target as StorySO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) StorySO.StoryTable.ConvertRow(row, t);
				else t.Reset();
			}
		}

		/*
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			///
			/// Automate the way we create stories
			///
			if(GUILayout.Button("Save and generate"))
			{
				var tableComponent = target as StorySO;
				// if we have not target or no actor or no story selected don't continue.
				if (!tableComponent || tableComponent.ID == UInt32.MaxValue)
				{
					Debug.LogError("We are missing a target or ID");
					return;
				}

				// create all the dialogue lines
				// get the first field
				TableRow row = TableDatabase.Get.GetRow("stories", tableComponent.ID);

				StorySO.StoryTable.ConvertRow(row, tableComponent);
				if (tableComponent && row != null)
				{
					// convert row data
					RenameAsset(target, tableComponent.Title);
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="nextDialogueID"></param>
		/// <returns></returns>
		private DialogueLineSO GetNextDialogue(string directory, UInt32 nextDialogueID)
		{
			DialogueLineSO dialogue = null;
			// See if we have other dialogue after this one
			if (nextDialogueID != UInt32.MaxValue)
			{
				dialogue = DialogueTable.ConvertRow(TableDatabase.Get.GetRow("dialogues", nextDialogueID));
				dialogue.ID = m_NextID;

				// Set the dialogue options associated to the dialogue.
				var optionsAssociated = CheckDialogueOptions(directory, dialogue);
				if (optionsAssociated.Count > 0) dialogue.Choices = optionsAssociated;
			}

			return dialogue;
		}

		private string RelativePath(string directory)
		{
			var relativePath = HelperClass.MakePathRelative(directory);
			Directory.CreateDirectory(relativePath);

			return relativePath;
		}

		private void SaveDialogueAsset(string directory, DialogueLine dialogueLineSo)
		{
			var relativePath = RelativePath(directory);
			var sharedDataPath = Path.Combine(relativePath, $"{dialogueLineSo.Actor.ActorName}_line_{dialogueLineSo.ID}.asset");
			sharedDataPath = AssetDatabase.GenerateUniqueAssetPath(sharedDataPath);
			SaveAsset(dialogueLineSo, sharedDataPath);
		}

		private void SaveDialogueOptionAsset(string directory, ActorSO actorSo, DialogueChoiceSO dialogueChoiceSo)
		{
			var relativePath = RelativePath(directory);

			var sharedDataPath = Path.Combine(relativePath, $"{actorSo.ActorName}_option_{dialogueChoiceSo.ID}.asset");
			sharedDataPath = AssetDatabase.GenerateUniqueAssetPath(sharedDataPath);
			SaveAsset(dialogueChoiceSo, sharedDataPath);
		}

		private void SaveAsset(UnityEngine.Object asset, string path)
		{
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.Refresh();
		}
		*/
	}

	[UnityEditor.CustomEditor(typeof(SkillSO))]
	public class SkillEditor : BaseTableEditor<SkillSO>
	{
		protected override void OnChanged()
		{
			var t = target as SkillSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) SkillSO.SkillTable.ConvertRow(row, t);
				else t.Reset();
			}
		}
	}


	[UnityEditor.CustomEditor(typeof(CharacterClassSO))]
	public class CharacterClassEditor : BaseTableEditor<CharacterClassSO>
	{
		protected override void OnChanged()
		{
			var t = target as CharacterClassSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) CharacterClassSO.ClassTable.ConvertRow(row, t);
				else t.Reset();
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(TaskSO))]
	public class TaskEditor : BaseTableEditor<TaskSO>
	{
		protected override void OnChanged()
		{
			var t = target as TaskSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) TaskSO.TaskTable.ConvertRow(row, t);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(ItemSO))]
	public class ItemEditor : BaseTableEditor<ItemSO>
	{
		protected override void OnChanged()
		{
			Debug.Log($"is JObject {IsJsonObj}");
			var t = target as ItemSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) ItemSO.ItemTable.ConvertRow(row, t);
				UnityEditor.EditorUtility.SetDirty(t);
				Repaint();
				Debug.Log("repaint");
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(ItemRecipeSO))]
	public class ItemRecipeEditor : BaseTableEditor<ItemRecipeSO>
	{
		protected override void OnChanged()
		{
			Debug.Log($"is JObject {IsJsonObj}");
			var t = target as ItemRecipeSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) ItemSO.ItemTable.ConvertRow(row, t);
				UnityEditor.EditorUtility.SetDirty(t);
				Repaint();
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(EnemySO))]
	public class EnemyEditor : BaseTableEditor<EnemySO>
	{
		protected override void OnChanged()
		{
			var t = target as EnemySO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) EnemyTable.ConvertRow(row, t);
			}
		}
	}
}
