using System;
using System.IO;
using UnityEditor;
using UnityEngine;

using StoryTime.Database.ScriptableObjects;
using StoryTime.Editor.Extensions;

namespace StoryTime.Editor.Wizards
{
	public abstract class BaseWizard<TW, T> : ScriptableWizard
		where TW: ScriptableWizard
		where T : TableBehaviour
	{
		internal static Action<T, string> OnCreate;

		public static void CreateWizard(string title, string createButtonName = "", string otherButtonName = "")
		{
			DisplayWizard<TW>(title, createButtonName, otherButtonName);
		}

		public void OnWizardCreate()
		{
			var path = EditorUtility.SaveFilePanel("Create new", Application.dataPath, "", "asset");
			if (string.IsNullOrEmpty(path))
				return;

			path = path.Substring(path.ToLower().IndexOf("assets/", StringComparison.Ordinal));
			T newItem = Create(path);
			AssetDatabase.CreateAsset(newItem, path);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();

			string withoutExtension = Path.GetFileNameWithoutExtension(path);
			path = path.Trim('/') + "/" + withoutExtension;
			EditorMenuTreeExtensions.SplitMenuPath(path, out path, out var itemName);

			OnCreate?.Invoke(newItem, itemName);
		}

		public virtual void OnWizardUpdate()
		{
			isValid = Validate();
		}

		public void OnWizardOtherButton() { }

		protected abstract bool Validate();
		protected abstract T Create(string location);
	}
}
