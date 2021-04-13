using UnityEngine;
using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	using Components;
	public class InspectorDescriptionFiller : MonoBehaviour
	{
		[SerializeField] private LocalizeStringEvent textName;
		[SerializeField] private LocalizeStringEvent textDescription;

		public void FillDescription(ItemStack itemToInspect)
        {
	        textName.StringReference = itemToInspect.Item.ItemName;
        	textName.StringReference.Arguments = new object[] { new { Purpose = 0, Amount = 1 } };
        	textDescription.StringReference = itemToInspect.Item.Description;

        	textName.gameObject.SetActive(true);
        	textDescription.gameObject.SetActive(true);
        }
	}
}
