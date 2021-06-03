using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DatabaseSync.Components
{
	using Binary;

	// [CreateAssetMenu(fileName = "newDialogueEvent", menuName = "DatabaseSync/Events/Narrative/Dialogue Event")]
	[Serializable]
	// ReSharper disable once InconsistentNaming
	public class DialogueEventSO /*: ScriptableObject */
	{
		public string EventName => eventName;

		public dynamic[] Values => values;

		[Tooltip("Dialogue Option Event name")]
		[SerializeField] private string eventName = String.Empty;

		[Tooltip("Dialogue Option Event value you want to pass")]
		[SerializeField] private dynamic[] values;

		public static DialogueEventSO ConvertRow(TableRow row, DialogueEventSO dialogueEvent = null)
		{
			DialogueEventSO eventSo = dialogueEvent ?? new DialogueEventSO();

			if (row.Fields.Count == 0)
            {
            	return eventSo;
            }

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("name"))
				{
					string data = (string) field.Value.Data;
					eventSo.eventName = data;
				}

				if (field.Key.Equals("inputs"))
				{
					Debug.Log(field.Value.Data);
					// uint data = (JArray) field.Value.Data;
					// dialogue.nextDialogueID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}

				/*
				"inputs": [
				{
					"defaultValue": 0,
					"paramName": "MyParamName",
					"value": 0
				},
				{
					"defaultValue": 0,
					"paramName": "Input In 4 - [NULL]",
					"value": 0
				}
				],
				*/
			}

			return eventSo;
		}
	}
}
