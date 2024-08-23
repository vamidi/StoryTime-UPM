using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Game.Interaction.UI
{
	[CreateAssetMenu(fileName = "interactionItem", menuName = "StoryTime/Game/UI/Interaction Item", order = 51)]
	public class InteractionItemSO : InteractionSO
	{
		[Tooltip("Item description")]
		[SerializeField] public LocalizedString interactionItemDescription;
	}
}
