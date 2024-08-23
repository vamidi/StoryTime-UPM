using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

namespace StoryTime.Domains.ItemManagement.UI
{
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
	
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
