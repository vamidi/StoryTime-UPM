using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

namespace StoryTime.Components.UI
{
	using Components;
	public class InspectorDescriptionFiller : MonoBehaviour
	{
		[SerializeField] private Image itemImage;
		[SerializeField] private LocalizeStringEvent textName;
		[SerializeField] private LocalizeStringEvent textDescription;

		public void FillDescription(ItemStack itemToInspect)
		{
			// TODO do something with localized images
			if(!itemToInspect.Item.IsLocalized)
				itemImage.sprite = itemToInspect.Item.PreviewImage;

	        textName.StringReference = itemToInspect.Item.ItemName;
        	textName.StringReference.Arguments = new object[] { new { Purpose = 0, Amount = 1 } };
        	textDescription.StringReference = itemToInspect.Item.Description;

        	textName.gameObject.SetActive(true);
        	textDescription.gameObject.SetActive(true);
        }
	}
}
