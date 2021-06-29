using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync
{
	[CreateAssetMenu(fileName = "interactionItem", menuName = "DatabaseSync/UI/Interaction Item", order = 51)]
	public class InteractionItemSO : InteractionSO
	{
		[Tooltip("Item description")]
		[SerializeField] public LocalizedString interactionItemDescription;
	}
}
