using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Editor.Domains.UI
{
	using StoryTime.Editor.Domains.Database;
	using StoryTime.Domains.Database.ScriptableObjects;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	using StoryTime.Domains.Narrative.Tasks.ScriptableObjects;
	using StoryTime.Domains.Game.NPC.Enemies.ScriptableObjects;
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	using StoryTime.FirebaseService.Database.ResourceManagement;
	using StoryTime.Domains.ItemManagement.Crafting.ScriptableObjects;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;

	public class BaseTableEditor<T> : UnityEditor.Editor where T : TableBehaviour
	{
		public int Choice
		{
			protected set => _choiceIndex = value;
			get => _choiceIndex;
		}

		protected bool IsJsonObj;

		private int _choiceIndex;
		private Dictionary<String, String> _populatedList = new()
		{
			{ "Empty", "None" }
		};


		public virtual void OnEnable()
		{
			DatabaseSyncModule.onFetchCompleted += (_, tableName) =>
			{
				var tblComp = target as TableBehaviour;
				if (tblComp != null && tblComp.TableName == tableName)
				{
					Debug.Log($"Re-generating list of {tableName}");
					GenerateList();
				}
			};
			GenerateList();
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
			_choiceIndex = UnityEditor.EditorGUILayout.Popup(arrayLabel, _choiceIndex, _populatedList.Values.ToArray());

			// Draw the default inspector
			base.OnInspectorGUI();
			//
			// DrawDefaultInspector();

			var t = target as TableBehaviour;
			if (_choiceIndex >= 0)
			{
				var newID = _populatedList.Keys.ToArray()[_choiceIndex];
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
			// set the id right
			Reset();
		}

		protected void Reset()
		{
			// set the id right
			var tblComp = target as TableBehaviour;
			if (tblComp != null)
			{
				_choiceIndex = Array.FindIndex(_populatedList.Keys.ToArray(), idx => idx == tblComp.ID);
				BaseEditorList.GenerateList(ref _populatedList, tblComp, out IsJsonObj);
			}

			Repaint();
		}
	}

	// [UnityEditor.CustomEditor(typeof(TableBehaviour), true)]
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
	// [UnityEditor.CustomEditor(typeof(CharacterSO))]
	public class ActorTableEditor : BaseTableEditor<CharacterSO>
	{
		public override void OnEnable()
		{
			base.OnEnable();
			GenerateList();
		}

		protected override void OnChanged()
		{
			var t = target as CharacterSO;
			if (t && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);
				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(StorySO))]
	public class StoryTableEditor : BaseTableEditor<StorySO>
	{
		private Dictionary<String, FirebaseStorageService.StoryFileUpload> stories = new()
		{
			{ "Empty", null }
		};

		protected override void GenerateList()
		{
			base.GenerateList();
			/*
			FirebaseInitializer.StorageService.GetFiles<FirebaseStorageService.StoryFileUpload>("stories")
				.ContinueWith(
					(task) =>
					{
						if (task.IsFaulted)
						{
							return;
						}

						var list = task.Result;

						foreach (var storyFileUpload in list)
						{
							if (stories.ContainsKey(storyFileUpload.storyId))
							{
								stories[storyFileUpload.storyId] = storyFileUpload;
							}
							else
							{
								stories.Add(storyFileUpload.storyId, storyFileUpload);
							}
						}

						Reset();
					}
				);
			*/
		}

		protected override void OnChanged()
		{
			var t = target as StorySO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
				// else t.Reset();

				Prompt(t);
			}
		}

		private void Prompt(StorySO t)
		{
			if ((t.rootNode || t.nodes.Value.Count > 0) &&
				UnityEditor.EditorUtility.DisplayDialog("Available Story",
				    "There is a current story available.\nAre you sure you want to delete the current one?", "Yes",
				    "No"))
			{
				if (!stories.ContainsKey(t.ID))
				{
					Debug.Log("Story could not be found!");
					return;
				}

				// Delete the start node
				t.rootNode = null;

				// Delete the rest of the nodes
				foreach (var node in t.nodes.Value)
				{
					// t.DeleteNode(node);
				}

				var story = stories[t.ID];
				// t.Parse(story);
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
	public class SkillTableEditor : BaseTableEditor<SkillSO>
	{
		protected override void OnChanged()
		{
			var t = target as SkillSO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
				// else t.Reset();
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(CharacterClassSO))]
	public class CharacterClassTableEditor : BaseTableEditor<CharacterClassSO>
	{
		protected override void OnChanged()
		{
			var t = target as CharacterClassSO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
				// else t.Reset();
			}
		}
	}

	// [UnityEditor.CustomEditor(typeof(TaskSO))]
	public class TaskTableEditor : BaseTableEditor<TaskSO>
	{
		protected override void OnChanged()
		{
			var t = target as TaskSO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
			}
		}
	}

	// [UnityEditor.CustomEditor(typeof(ItemSO))]
	public class ItemTableEditor : BaseTableEditor<ItemSO>
	{
		protected override void OnChanged()
		{
			Debug.Log($"is JObject {IsJsonObj}");
			var t = target as ItemSO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
				UnityEditor.EditorUtility.SetDirty(t);
				Repaint();
				Debug.Log("repaint");
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(ItemRecipeSO))]
	public class ItemRecipeTableEditor : BaseTableEditor<ItemRecipeSO>
	{
		protected override void OnChanged()
		{
			Debug.Log($"is JObject {IsJsonObj}");
			var t = target as ItemRecipeSO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
				UnityEditor.EditorUtility.SetDirty(t);
				Repaint();
			}
		}
	}

	[UnityEditor.CustomEditor(typeof(EnemySO))]
	public class EnemyTableEditor : BaseTableEditor<EnemySO>
	{
		protected override void OnChanged()
		{
			var t = target as EnemySO;
			if (t != null && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);

				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
			}
		}
	}
}
