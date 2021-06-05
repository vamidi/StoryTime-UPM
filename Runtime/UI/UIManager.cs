using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;
	using Events;

	public class UIManager : MonoBehaviour
	{
		[SerializeField] private Input.BaseInputReader inputReader;

		[Header("Listening on channels")]

		[Header("Dialogue Events")]
		[SerializeField] private DialogueLineChannelSO openUIDialogueEvent;

		[SerializeField] private VoidEventChannelSO closeUIDialogueEvent;

		[Header("Inventory Events")]
		[SerializeField] private VoidEventChannelSO openInventoryScreenEvent;
		[SerializeField] private VoidEventChannelSO closeInventoryScreenEvent;

		[Header("Crafting/Cooking Events")]
		[SerializeField] private CollectionEventChannelSO openInventoryScreenForCookingEvent;
		[SerializeField] private VoidEventChannelSO closeInventoryScreenForCookingEvent;

		[Header("Interaction Events")]
		[SerializeField] private VoidEventChannelSO onInteractionEndedEvent;

		[SerializeField] private InteractionUIEventChannelSO setInteractionEvent;

		[Header("Navigation Events")]
		[SerializeField] private InteractionStoryUIEventChannel showNavigationInteractionEvent;

		[SerializeField] private InteractionItemUIEventChannel showItemInteractionEvent;

		[Header("Story Manager Events")]
		[SerializeField] private VoidEventChannelSO openStoryScreenEvent;
		[SerializeField] private VoidEventChannelSO closeStoryScreenEvent;

		[Header("References")]
		[SerializeField] private UIDialogueManager dialogueController;

		[SerializeField] private UIInventoryManager inventoryPanel;
		[SerializeField] private UICraftingManager craftingPanel;

		[SerializeField] private UIStoryManager storyManagerPanel;

		[SerializeField] private UIInteractionManager interactionPanel;

		[SerializeField] private UIInteractionNavManager navigationPanel;

		[SerializeField] private UIInteractionItemManager interactionItemPanel;

		[Header("Broadcasting channels")]
		[SerializeField] private VoidEventChannelSO storyScreenClosedEvent;

		private bool m_IsForCookingOrCraft;

		protected void OnEnable()
		{
			// Check if the event exists to avoid errors
			if (openUIDialogueEvent != null)
				openUIDialogueEvent.OnEventRaised += OpenUIDialogue;

			if (closeUIDialogueEvent != null)
				closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;

			if (openInventoryScreenForCookingEvent != null)
				openInventoryScreenForCookingEvent.OnRecipeEventRaised += SetInventoryScreenForCooking;

			if (closeInventoryScreenForCookingEvent != null)
				closeInventoryScreenForCookingEvent.OnEventRaised += CloseInventoryScreen;

			if (openInventoryScreenEvent != null)
				openInventoryScreenEvent.OnEventRaised += SetInventoryScreen;

			if (closeInventoryScreenEvent != null)
				closeInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;

			if (openStoryScreenEvent != null)
				openStoryScreenEvent.OnEventRaised += OpenStoryScreen;

			if (closeStoryScreenEvent != null)
				closeStoryScreenEvent.OnEventRaised += CloseStoryScreen;

			if (setInteractionEvent != null)
				setInteractionEvent.OnEventRaised += SetInteractionPanel;

			if (showNavigationInteractionEvent != null)
				showNavigationInteractionEvent.OnEventRaised += ShowNavigationPanel;

			if (showNavigationInteractionEvent != null)
				showNavigationInteractionEvent.OnEventRaised += ShowNavigationPanel;

			if (showItemInteractionEvent != null)
				showItemInteractionEvent.OnEventRaised += ShowItemPanel;

			if (inputReader) inputReader.menuCancelEvent += CloseStoryScreen;
		}

		private void Start()
		{
			CloseUIDialogue();
			CloseStoryScreen();
		}

		public void OpenUIDialogue(IDialogueLine dialogueLine, ActorSO actor)
		{
			dialogueController.SetDialogue(dialogueLine, actor);
			dialogueController.gameObject.SetActive(true);
		}

		public void CloseUIDialogue()
		{
			dialogueController.gameObject.SetActive(false);
		}

		public void SetInventoryScreenForCooking(RecipeCollectionSO recipeCollection)
		{
			m_IsForCookingOrCraft = true;
			OpenInventoryScreen(recipeCollection);
		}

		public void SetInventoryScreen()
		{
			m_IsForCookingOrCraft = false;
			// TODO change when we need multiple bags.
			OpenInventoryScreen(null);
		}

		protected void OpenInventoryScreen(object inventory)
		{
			TabType tabType = TabType.None;

			if (m_IsForCookingOrCraft)
			{
				tabType = TabType.Recipes;

				craftingPanel.SetInventory = inventory as RecipeCollectionSO;
				craftingPanel.gameObject.SetActive(true);
				craftingPanel.FillInventory(tabType);
				return;
			}

			inventoryPanel.gameObject.SetActive(true);
			inventoryPanel.FillInventory(tabType);
		}


		public void CloseInventoryScreen()
		{
			if (m_IsForCookingOrCraft)
			{
				craftingPanel.gameObject.SetActive(false);
				onInteractionEndedEvent.RaiseEvent();

				return;
			}
			inventoryPanel.gameObject.SetActive(false);
			onInteractionEndedEvent.RaiseEvent();
		}

		public void OpenStoryScreen()
		{
			storyManagerPanel.ShowCategories();
			storyManagerPanel.gameObject.SetActive(true);
		}

		public void CloseStoryScreen()
		{
			storyManagerPanel.HideCategories();
			storyManagerPanel.gameObject.SetActive(false);

			if (storyScreenClosedEvent != null) storyScreenClosedEvent.RaiseEvent();
		}

		public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
		{
			if (isOpenEvent)
			{
				interactionPanel.FillInteractionPanel(interactionType);
			}

			interactionPanel.gameObject.SetActive(isOpenEvent);
		}

		public void ShowNavigationPanel(bool isOpenEvent, StoryInfo info, InteractionType interactionType)
		{
			if (isOpenEvent)
			{
				navigationPanel.SetQuest(info, interactionType);
				navigationPanel.FillInteractionPanel(interactionType);
			}

			navigationPanel.gameObject.SetActive(isOpenEvent);
		}

		public void ShowItemPanel(bool isOpenEvent, ItemStack itemInfo, InteractionType interactionType)
		{
			if (isOpenEvent)
			{
				interactionItemPanel.SetItem(itemInfo);
				interactionItemPanel.FillInteractionPanel(interactionType);
			}

			interactionItemPanel.gameObject.SetActive(isOpenEvent);
		}
	}
}
