using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Domains.Narrative.UI.Dialogues
{
	using StoryTime.Domains.Narrative.Dialogues;
	
	public class UIDialogueChoicesManager : MonoBehaviour
	{
		public UIDialogueChoiceFiller[] listChoiceButtons;

		public void FillChoices(List<DialogueChoice> choices)
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
						UnityEngine.Debug.LogError("There are more choices than buttons");
					}
				}
			}
		}
	}
}

