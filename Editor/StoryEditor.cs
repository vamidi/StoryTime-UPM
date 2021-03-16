using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.Editor
{
	using Database;

	public class BaseTableEditor<T> : UnityEditor.Editor where T : Components.TableBehaviour
	{
		protected int Choice
		{
			set => _choiceIndex = value;
		}

		private int _choiceIndex = Int32.MaxValue;

		protected Dictionary<uint, string> PopulatedList = new Dictionary<uint, string>();

		public virtual void OnEnable()
		{
			DatabaseSyncModule.FetchCompleted += (o, args) =>
			{
				TableDatabase.Get.Refresh();
				GenerateList();
			};
			GenerateList();

			// set the id right
			var t = target as Components.TableBehaviour;
			if (t) _choiceIndex = (int) t.ID;
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
			// Draw the default inspector
			DrawDefaultInspector();
			GUIContent arrayLabel = new GUIContent("ID");
			_choiceIndex = UnityEditor.EditorGUILayout.Popup(arrayLabel, _choiceIndex, PopulatedList.Values.ToArray());
			var t = target as Components.TableBehaviour;
			if (t && t.ID != _choiceIndex)
			{
				// Update the selected choice in the underlying object
				t.ID = (uint) _choiceIndex;
				OnChanged();

				// Save the changes back to the object
				UnityEditor.EditorUtility.SetDirty(target);
			}
		}

		protected virtual void GenerateList()
		{
			var tableComponent = target as Components.TableBehaviour;
			if (tableComponent != null)
			{
				_choiceIndex = (int) tableComponent.ID;

				var binary = TableDatabase.Get.GetBinary(tableComponent.Name);
				string linkColumn = tableComponent.LinkedColumn;
				uint linkId = tableComponent.LinkedID;
				PopulatedList = linkColumn != "" && linkId != UInt32.MaxValue ? binary.PopulateWithLink(
					tableComponent.DropdownColumn,
					linkColumn,
					linkId
				) : binary.Populate(tableComponent.DropdownColumn);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(Components.TableBehaviour), true)]
	public class TableEditor : BaseTableEditor<Components.TableBehaviour>
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
	[UnityEditor.CustomEditor(typeof(Components.ActorSO))]
	public class ActorEditor : BaseTableEditor<Components.ActorSO>
	{
		public override void OnEnable()
		{
			base.OnEnable();
			GenerateList();
		}

		protected override void OnChanged()
		{
			var t = target as Components.ActorSO;
			if (t && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);
				if (row != null)
				{
					// set all the values from the selected row
					// t = CharacterTable.ConvertRow(row);
					RenameAsset(target, t.ActorName);
				}
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(Components.StorySO))]
	public class StoryEditor : BaseTableEditor<Components.StorySO>
	{
		protected override void OnChanged()
		{
			var t = target as Components.StorySO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) Components.StorySO.StoryTable.ConvertRow(row, t);
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

	[UnityEditor.CustomEditor(typeof(Components.DialogueLineSO))]
	public class DialogueEditor : BaseTableEditor<Components.DialogueLineSO>
	{
		protected override void OnChanged()
		{
			var t = target as Components.DialogueLineSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) Components.DialogueLineSO.ConvertRow(row, t);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(Components.TaskSO))]
	public class TaskEditor : BaseTableEditor<Components.TaskSO>
	{
		protected override void OnChanged()
		{
			var t = target as Components.TaskSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) Components.TaskTable.ConvertRow(row, t);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(Components.ItemSO))]
	public class ItemEditor : BaseTableEditor<Components.ItemSO>
	{
		protected override void OnChanged()
		{
			var t = target as Components.ItemSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) Components.ItemTable.ConvertRow(row, t);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(Components.EnemySO))]
	public class EnemyEditor : BaseTableEditor<Components.EnemySO>
	{
		protected override void OnChanged()
		{
			var t = target as Components.EnemySO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null) Components.EnemyTable.ConvertRow(row, t);
			}
		}
	}
}
