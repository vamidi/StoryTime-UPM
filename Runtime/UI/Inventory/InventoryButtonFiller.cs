using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
namespace StoryTime.Components.UI
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
