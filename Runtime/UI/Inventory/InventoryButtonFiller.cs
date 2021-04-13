using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	public class InventoryButtonFiller : MonoBehaviour
	{
		[SerializeField] private LocalizeStringEvent buttonActionText;

		[SerializeField] private Button buttonAction;

		public void FillInventoryButtons(ItemTypeSO itemType, bool isInteractable = true)
		{
			buttonAction.interactable = isInteractable;
			buttonActionText.StringReference = itemType.ActionName;
		}
	}
}
