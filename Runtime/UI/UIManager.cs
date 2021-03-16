using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;
	using Events;

	public class UIManager : MonoBehaviour
	{
		[SerializeField] private Input.InputReader inputReader;

		[Header("Listening on channels")]
		[Header("Dialogue Events")]
		public DialogueLineChannelSO openUIDialogueEvent;
		public VoidEventChannelSO closeUIDialogueEvent;

		[Header("Inventory Events")]
		public VoidEventChannelSO openInventoryScreenEvent;
		public VoidEventChannelSO openInventoryScreenForCookingEvent;

		[Header("Interaction Events")]
		public VoidEventChannelSO onInteractionEndedEvent;
		public InteractionUIEventChannelSO setInteractionEvent;

		[Header("Navigation Events")]
		public InteractionStoryUIEventChannel showNavigationInteractionEvent;
		public InteractionItemUIEventChannel showItemInteractionEvent;

		[Header("Story Manager Events")]
		public VoidEventChannelSO openStoryScreenEvent;
		public VoidEventChannelSO closeStoryScreenEvent;

		[Header("References")]

		[SerializeField] UIDialogueManager dialogueController;

		// [SerializeField] UIInventoryManager inventoryPanel;

		[SerializeField] UIStoryManager storyManagerPanel;

		[SerializeField] private UIInteractionManager interactionPanel;

		[SerializeField] private UIInteractionNavManager navigationPanel;

		[SerializeField] private UIInteractionItemManager interactionItemPanel;

		[Header("Broadcasting channels")]
		[SerializeField] private VoidEventChannelSO closeInventoryScreenEvent;
		[SerializeField] private VoidEventChannelSO storyScreenClosedEvent;


		bool m_IsForCooking;

		private void OnEnable()
		{
			// Check if the event exists to avoid errors
			if (openUIDialogueEvent != null)
				openUIDialogueEvent.OnEventRaised += OpenUIDialogue;

			if (closeUIDialogueEvent != null)
				closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;

			if (openInventoryScreenForCookingEvent != null)
				openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;

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

			if(inputReader) inputReader.menuCancelEvent += CloseStoryScreen;
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

		public void SetInventoryScreenForCooking()
		{
			m_IsForCooking = true;
			OpenInventoryScreen();
		}

		public void SetInventoryScreen()
		{
			m_IsForCooking = false;
			OpenInventoryScreen();
		}

		void OpenInventoryScreen()
		{
			// inventoryPanel.gameObject.SetActive(true);

			if (m_IsForCooking)
			{
				// inventoryPanel.FillInventory(TabType.recipe, true);
			}
			else
			{
				// inventoryPanel.FillInventory();
			}
		}


		public void CloseInventoryScreen()
		{
			// inventoryPanel.gameObject.SetActive(false);

			if (m_IsForCooking)
			{
				onInteractionEndedEvent.RaiseEvent();
			}
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

			if(storyScreenClosedEvent != null) storyScreenClosedEvent.RaiseEvent();

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
				navigationPanel.SetQuest(info);
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
