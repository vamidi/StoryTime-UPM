﻿using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync
{
	using Components;
	public class UIDialogueChoicesManager : MonoBehaviour
	{
		public UIDialogueChoiceFiller[] listChoiceButtons;

		public void Start()
		{
			foreach (var uiDialogueChoiceFiller in listChoiceButtons)
			{
				uiDialogueChoiceFiller.gameObject.SetActive(false);
			}
		}

		public void FillChoices(List<DialogueChoiceSO> choices)
		{

			if (choices != null)
			{
				int maxCount = Mathf.Max(choices.Count, listChoiceButtons.Length);

				for (int i = 0; i < maxCount; i++)
				{
					if (i < listChoiceButtons.Length)
					{
						if (i < choices.Count)
						{
							listChoiceButtons[i].FillChoice(choices[i]);
							listChoiceButtons[i].gameObject.SetActive(true);
						}
						else
						{
							listChoiceButtons[i].gameObject.SetActive(false);
						}
					}
					else
					{
						Debug.LogError("There are more choices than buttons");
					}
				}
			}
		}
	}
}

