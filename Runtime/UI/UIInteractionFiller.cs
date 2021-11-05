using UnityEngine;
using TMPro;

namespace StoryTime.Components.UI
{
	public class UIInteractionFiller : BaseUIInteractionItemFiller<ScriptableObjects.InteractionSO>
	{
		public override void FillInteractionPanel(ScriptableObjects.InteractionSO interactionItem)
		{
			// StringReference normally fire the update event and also updates the interaction title.
			interactionName.StringReference = interactionItem.InteractionName;
			interactionKeyButton.text = KeyCode.E.ToString(); // this keycode will be modified later on
		}
	}
}
