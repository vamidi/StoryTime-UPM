using UnityEngine;

namespace DatabaseSync
{
	using Components;

	public class UIManager : MonoBehaviour
	{

		public DialogueLineChannelSO _openUIDialogueEvent;

		public VoidEventChannelSO CloseUIDialogueEvent;
		public VoidEventChannelSO OpenInventoryScreenEvent;
		public VoidEventChannelSO OpenInventoryScreenForCookingEvent;
		public VoidEventChannelSO CloseInventoryScreenEvent;
		public VoidEventChannelSO OnInteractionEndedEvent;

		public InteractionUIEventChannelSO SetInteractionEvent;

		[SerializeField] UIDialogueManager dialogueController = default;

		// [SerializeField] UIInventoryManager inventoryPanel;

		[SerializeField]
		private UIInteractionManager interactionPanel = default;

		private void OnEnable()
		{
			//Check if the event exists to avoid errors
			if (_openUIDialogueEvent != null)
			{
				_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
			}

			if (CloseUIDialogueEvent != null)
			{
				CloseUIDialogueEvent.OnEventRaised += CloseUIDialogue;
			}

			if (OpenInventoryScreenForCookingEvent != null)
			{
				OpenInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
			}

			if (OpenInventoryScreenEvent != null)
			{
				OpenInventoryScreenEvent.OnEventRaised += SetInventoryScreen;
			}

			if (CloseInventoryScreenEvent != null)
			{
				CloseInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;
			}

			if (SetInteractionEvent != null)
			{
				SetInteractionEvent.OnEventRaised += SetInteractionPanel;
			}

		}

		private void Start()
		{
			CloseUIDialogue();
		}

		public void OpenUIDialogue(DialogueLineSO dialogueLine, ActorSO actor)
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
			isForCooking = true;
			OpenInventoryScreen();

		}

		public void SetInventoryScreen()
		{
			isForCooking = false;
			OpenInventoryScreen();

		}

		bool isForCooking = false;

		void OpenInventoryScreen()
		{
			/*
			inventoryPanel.gameObject.SetActive(true);

			if (isForCooking)
			{
				inventoryPanel.FillInventory(TabType.recipe, true);

			}
			else
			{
				inventoryPanel.FillInventory();
			}
			*/
		}


		public void CloseInventoryScreen()
		{
			/*
			inventoryPanel.gameObject.SetActive(false);

			if (isForCooking)
			{
				OnInteractionEndedEvent.RaiseEvent();
			}
			*/
		}

		public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
		{
			if (isOpenEvent)
			{
				interactionPanel.FillInteractionPanel(interactionType);
			}

			interactionPanel.gameObject.SetActive(isOpenEvent);
		}
	}
}
