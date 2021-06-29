using DatabaseSync.Components;

namespace DatabaseSync.UI
{
	public class UIInteractionItemManager : BaseUIInteractionManager<InteractionItemSO, UIInteractionItemFiller>
	{
		public void SetItem(ItemStack itemStack, InteractionType interactionType)
		{
			if (listInteractions != null && interactionItem != null)
			{
				if (listInteractions.Exists(o => o.InteractionType == interactionType))
				{
					var interaction = listInteractions.Find(o =>
						o.InteractionType == interactionType);

					if (interaction != null)
					{
						interaction.InteractionName = itemStack.Item.ItemName;
						interaction.interactionItemDescription = itemStack.Item.Description;
					}
				}
			}
		}
	}
}
