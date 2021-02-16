using System;
using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.Components
{

	/// <summary>
	/// DialogueLine is a Scriptable Object that represents one line of spoken dialogue.
	/// It references the Actor that speaks it.
	/// </summary>
	[CreateAssetMenu(fileName = "newLineOfDialogue", menuName = "DatabaseSync/Dialogues/Dialogue")]
	// ReSharper disable once InconsistentNaming
	public class DialogueLineSO : TableBehaviour
	{
		public string Sentence => Text;

		/// <summary>
		///
		/// </summary>
		[SerializeField, HideInInspector]
		public uint ParentID = UInt32.MaxValue;

		/// <summary>
		///
		/// </summary>
		[SerializeField, HideInInspector]
		public uint NextDialogueID = UInt32.MaxValue;

		/// <summary>
		///
		/// </summary>
		[SerializeField, HideInInspector]
		public uint CharacterID = UInt32.MaxValue;

		/// <summary>
		///
		/// </summary>
		[SerializeField, HideInInspector]
		public string Text = "";

		[SerializeField]
		private ActorSO actor;

		[SerializeField]
		private List<DialogueChoiceSO> choices;

		public List<DialogueChoiceSO> Choices
		{
			get => choices;
			set => choices = value;
		}

		public ActorSO Actor
		{
			get => actor;
			set => actor = value;
		}

		public DialogueLineSO() : base("dialogues", "text", "parentId") { }
	}
}
