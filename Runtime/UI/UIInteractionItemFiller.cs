using UnityEngine;
using TMPro;

namespace DatabaseSync
{
	public class UIInteractionItemFiller : MonoBehaviour
	{
		// TODO make multi language and change it back to string reference
		[SerializeField] TextMeshProUGUI interactionName = default;

		[SerializeField] TextMeshProUGUI interactionKeyButton = default;

		public void FillInteractionPanel(InteractionSO interactionItem)
		{
			// StringReference normally fire the update event and also updates the interaction title.
			interactionName.text /* .StringReference */ = interactionItem.InteractionName;
			interactionKeyButton.text = KeyCode.E.ToString(); // this keycode will be modified later on
		}
	}
}
