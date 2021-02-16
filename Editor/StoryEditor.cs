using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using DatabaseSync.Database;

namespace DatabaseSync.Components
{
	using Binary;
	using ResourceManagement.Util;

	public class BaseTableEditor<T> : Editor where T : TableBehaviour
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
			var t = target as TableBehaviour;
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
			string assetPath = AssetDatabase.GetAssetPath(t.GetInstanceID());
			AssetDatabase.RenameAsset(assetPath, fileName);
			AssetDatabase.SaveAssets();
		}

		protected virtual void OnChanged() { }

		public override void OnInspectorGUI()
		{
			// Draw the default inspector
			DrawDefaultInspector();
			GUIContent arrayLabel = new GUIContent("ID");
			_choiceIndex = EditorGUILayout.Popup(arrayLabel, _choiceIndex, PopulatedList.Values.ToArray());
			var t = target as TableBehaviour;
			if (t && t.ID != _choiceIndex)
			{
				// Update the selected choice in the underlying object
				t.ID = (uint) _choiceIndex;
				OnChanged();
			}

			// Save the changes back to the object
			EditorUtility.SetDirty(target);
		}

		protected virtual void GenerateList()
		{
			var tableComponent = target as TableBehaviour;
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

	[CustomEditor(typeof(TableBehaviour), true)]
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
	[CustomEditor(typeof(ActorSO))]
	public class ActorEditor : BaseTableEditor<ActorSO>
	{
		public override void OnEnable()
		{
			base.OnEnable();
			GenerateList();
		}

		protected override void OnChanged()
		{
			var t = target as ActorSO;
			if (t)
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

	[CustomEditor(typeof(StorySO))]
	public class StoryEditor : BaseTableEditor<StorySO>
	{
		private uint m_NextID = UInt32.MaxValue;

		private ActorSO Actor
		{
			get
			{
				var story = target as StorySO;
				return story ? story.Actor : null;
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			/*
			 * Automate the way we create stories
			 */
			if(GUILayout.Button("Generate Dialogues"))
			{
				var tableComponent = target as StorySO;
				// if we have not target or no actor or no story selected don't continue.
				if (!tableComponent || !tableComponent.Actor || tableComponent.ID == UInt32.MaxValue)
					return;

				// add everything the button would do.
				var assetDirectory = EditorUtility.SaveFolderPanel("Create Story Data", AssetDatabase.GetAssetPath(tableComponent), "");
				if (string.IsNullOrEmpty(assetDirectory))
					return;

				// create all the dialogue lines
				// get the first field
				TableRow row = TableDatabase.Get.GetRow("stories", tableComponent.ID);

				StoryTable.ConvertRow(row, tableComponent);
				if (tableComponent && row != null)
				{
					// convert row data
					RenameAsset(target, tableComponent.Title);

					// Get the first dialogue
					// TODO use custom scripter
					DialogueLineSO startDialogue = DialogueTable.ConvertRow(TableDatabase.Get.GetRow("dialogues", tableComponent.ChildId));

					startDialogue.Actor = tableComponent.Actor;

					m_NextID = startDialogue.NextDialogueID;
					SaveDialogueAsset(assetDirectory, startDialogue);

					tableComponent.AddDialogueLine(startDialogue);

					while (m_NextID != UInt32.MaxValue)
					{
						var nextDialogue = GetNextDialogue(assetDirectory, m_NextID);
						if (nextDialogue)
						{
							nextDialogue.Actor = tableComponent.Actor;
							SaveDialogueAsset(assetDirectory, nextDialogue);
							tableComponent.AddDialogueLine(nextDialogue);
							m_NextID = nextDialogue.NextDialogueID;
						}
						else
							m_NextID = UInt32.MaxValue;
					}
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

				// Set the dialogue options associated to the dialogue.
				var optionsAssociated = CheckDialogueOptions(directory, dialogue);
				if (optionsAssociated.Count > 0) dialogue.Choices = optionsAssociated;
			}

			return dialogue;
		}

		/// <summary>
		/// Grab the dialogue options from the database
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="dialogueData"></param>
		/// <returns></returns>
		private List<DialogueChoiceSO> CheckDialogueOptions(string directory, DialogueLineSO dialogueData)
		{
			List<DialogueChoiceSO> options = new List<DialogueChoiceSO>();

			// if we have a dialogue after this then return the dialogue
			if (dialogueData.NextDialogueID == UInt32.MaxValue)
			{
				// TODO cache this into DialogueDataSO
				// find if we have a dialogue option to store
				List<Tuple<uint, TableRow>> dialogueOptions = TableDatabase.Get.FindLinks("dialogueOptions", "parentId", dialogueData.ID);

				if (dialogueOptions.Count > 0)
				{
					foreach (var dialogueOptionRow in dialogueOptions)
					{
						// grab the dialogue option
						DialogueChoiceSO dialogueOptionSo = DialogueOptionTable.ConvertRow(dialogueOptionRow.Item2);

						// set the index ID
						dialogueOptionSo.ID = dialogueOptionRow.Item1;

						// Save the dialogue option to file.
						SaveDialogueOptionAsset(directory, Actor, dialogueOptionSo);

						// Add the dialogue option to the sequence as well.
						options.Add(dialogueOptionSo);
					}
				}
			}

			return options;
		}

		private string RelativePath(string directory)
		{
			var relativePath = HelperClass.MakePathRelative(directory);
			Directory.CreateDirectory(relativePath);

			return relativePath;
		}

		private void SaveDialogueAsset(string directory, DialogueLineSO dialogueLineSo)
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
	}

	[CustomEditor(typeof(DialogueChoiceSO))]
	public class DialogueOptionEditor : BaseTableEditor<DialogueChoiceSO>
	{
		protected override void OnChanged()
		{
			var t = target as DialogueChoiceSO;
			if (t != null && t.ID != UInt32.MaxValue)
			{
				var row = TableDatabase.Get.GetRow(t.Name, t.ID);

				// set all the values from the selected row
				if (row != null)
					DialogueOptionTable.ConvertRow(row, t);
			}
		}
	}
}
