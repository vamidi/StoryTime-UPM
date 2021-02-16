using System;
using UnityEngine;

namespace DatabaseSync.Components
{
	[CreateAssetMenu(fileName = "newOptionOfDialogue", menuName = "DatabaseSync/Dialogues/Dialogue Option")]
	// ReSharper disable once InconsistentNaming
	public class DialogueChoiceSO : TableBehaviour
	{
		/// <summary>
		/// The next dialogue id
		/// </summary>
		[SerializeField]
		public uint ChildId = UInt32.MaxValue;

		/// <summary>
		/// The dialogue id the dialogue option belongs to.
		/// </summary>
		[SerializeField]
		public uint ParentID = UInt32.MaxValue;

		/// <summary>
		/// The text we use to display.
		/// </summary>
		[SerializeField]
		public string Text = "";

		[SerializeField]
		public DialogueLineSO NextDialogue;

		[SerializeField]
		public DialogueChoiceEventChannelSO OptionEvent;

		[SerializeField]
		public ChoiceActionType ActionType;

		public string Response => Text;

		public DialogueChoiceSO() : base("dialogueOptions", "text") { }
	}
}
