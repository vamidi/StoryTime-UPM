using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace StoryTime.Domains.UI
{
	using Events.ScriptableObjects;
	using StoryTime.Domains.Game.Interaction;
	using StoryTime.Domains.Game.Characters.UI;
	using StoryTime.Domains.Game.Interaction.UI;
	using StoryTime.Domains.Narrative.Dialogues;
	using StoryTime.Domains.Narrative.UI.Stories;
	using StoryTime.Domains.Narrative.UI.Dialogues;
	using StoryTime.Domains.ItemManagement.Inventory;
	using StoryTime.Domains.ItemManagement.UI.Crafting;
	using StoryTime.Domains.Events.ScriptableObjects.UI;
	using StoryTime.Domains.ItemManagement.UI.Inventory;
	using StoryTime.Domains.Game.Input.ScriptableObjects;
	using StoryTime.Domains.Narrative.Dialogues.Events.UI;
	using StoryTime.Domains.Game.Interaction.UI.Navigation;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	using StoryTime.Domains.Game.Characters.ScriptableObjects.Events;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
	using StoryTime.Domains.Game.Interaction.UI.Events.ScriptableObjects;
	
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private BaseInputReader inputReader;

		[Header("Listening on channels")]

		[Header("Dialogue Events")]
		[SerializeField] private DialogueLineChannelSO openUIDialogueEvent;

		[SerializeField] private VoidEventChannelSO closeUIDialogueEvent;

		[Header("Inventory Events")]
		[SerializeField] private VoidEventChannelSO openInventoryScreenEvent;
		[SerializeField] private VoidEventChannelSO closeInventoryScreenEvent;

		[Header("Crafting/Cooking Events")]
		[SerializeField] private VoidEventChannelSO openInventoryScreenForCookingEvent;
		[SerializeField] private VoidEventChannelSO closeInventoryScreenForCookingEvent;

		[Header("Stat events")]
		[SerializeField] private CharacterEventChannelSO openStatsScreenEvent;

		[Header("Interaction Events")]
		[SerializeField] private VoidEventChannelSO onInteractionEndedEvent;

		[SerializeField] private InteractionUIEventChannelSO toggleInteractionEvent;

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
		[SerializeField] private UIStatsManager statsPanel;

		[SerializeField] private UIStoryManager storyManagerPanel;

		[SerializeField] private UIInteractionManager interactionPanel;

		[SerializeField] private UIInteractionNavManager navigationPanel;

		[SerializeField] private UIInteractionItemManager interactionItemPanel;

		[Header("Broadcasting channels")]
		[SerializeField] private VoidEventChannelSO storyScreenClosedEvent;

		private bool m_IsForCookingOrCraft;
		private bool m_IsAnimating;

		protected void OnEnable()
		{
			// Check if the event exists to avoid errors
			if (openUIDialogueEvent != null)
				openUIDialogueEvent.OnEventRaised += OpenUIDialogue;

			if (closeUIDialogueEvent != null)
				closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;

			if (openInventoryScreenForCookingEvent != null)
				openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;

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

			if (toggleInteractionEvent != null)
				toggleInteractionEvent.OnEventRaised += SetInteractionPanel;

			if (showNavigationInteractionEvent != null)
				showNavigationInteractionEvent.OnEventRaised += ShowNavigationPanel;

			if (showItemInteractionEvent != null)
			{
				showItemInteractionEvent.OnEventRaised += ShowItemPanel;
				showItemInteractionEvent.OnEventsRaised += ShowItemPanel;
			}

			if (openStatsScreenEvent != null)
				openStatsScreenEvent.OnEventRaised += ShowStatsPanel;

			if (inputReader) inputReader.menuCancelEvent += CloseStoryScreen;
		}

		private void Start()
		{
			CloseUIDialogue();
			CloseStoryScreen();
		}

		public void OpenUIDialogue(DialogueLine dialogueLine)
		{
			if (dialogueController)
			{
				dialogueController.SetDialogue(dialogueLine);
				dialogueController.gameObject.SetActive(true);
			}
		}

		public void CloseUIDialogue()
		{
			if(dialogueController)
				dialogueController.gameObject.SetActive(false);
		}

		public void SetInventoryScreenForCooking()
		{
			m_IsForCookingOrCraft = true;
			OpenInventoryScreen();
		}

		public void SetInventoryScreen()
		{
			m_IsForCookingOrCraft = false;
			// TODO change when we need multiple bags.
			OpenInventoryScreen();
		}

		protected void OpenInventoryScreen()
		{
			TabType tabType = TabType.None;

			if (m_IsForCookingOrCraft)
			{
				tabType = TabType.Recipes;

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
			if (storyManagerPanel)
			{
				storyManagerPanel.ShowCategoriesByTypeOrAll();
				storyManagerPanel.gameObject.SetActive(true);
			}
		}

		public void CloseStoryScreen()
		{
			if (storyManagerPanel)
			{
				storyManagerPanel.HideCategories();
				storyManagerPanel.gameObject.SetActive(false);

				if (storyScreenClosedEvent != null) storyScreenClosedEvent.RaiseEvent();
			}
		}

		public void ShowStatsPanel(CharacterSO characterToInspect)
		{
			if (statsPanel)
			{
				statsPanel.SetCharacter = characterToInspect;
				statsPanel.gameObject.SetActive(true);
				statsPanel.FillStats();
			}
		}

		public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
		{
			if (interactionPanel)
			{
				if (isOpenEvent)
				{
					interactionPanel.FillInteractionPanel(interactionType);
				}

				interactionPanel.gameObject.SetActive(isOpenEvent);
			}
		}

		public void ShowNavigationPanel(bool isOpenEvent, StoryInfo info, InteractionType interactionType)
		{
			if (interactionPanel)
			{
				if (isOpenEvent)
				{
					navigationPanel.SetQuest(info, interactionType);
					navigationPanel.FillInteractionPanel(interactionType);

					navigationPanel.transform.DOMoveX(200, 1.0f).SetEase(Ease.InOutQuad).OnComplete(
						() => navigationPanel.transform.DOMoveX(200, 5.0f).SetEase(Ease.InOutQuad).OnComplete(
							() => navigationPanel.transform.DOMoveX(-200, 1.0f).SetEase(Ease.InOutQuad).OnComplete(
								() => navigationPanel.gameObject.SetActive(false)
							)
						)
					);
				}

				navigationPanel.gameObject.SetActive(isOpenEvent);
			}
		}

		/// <summary>
		/// TODO make this in a queue form for multiple items
		/// </summary>
		/// <param name="isOpenEvent"></param>
		/// <param name="itemInfo"></param>
		/// <param name="interactionType"></param>
		/// <returns></returns>
		public void ShowItemPanel(bool isOpenEvent, ItemStack itemInfo, InteractionType interactionType)
		{
			if (isOpenEvent)
			{
				interactionItemPanel.SetItem(itemInfo, interactionType);
				interactionItemPanel.FillInteractionPanel(interactionType);

				if (!m_IsAnimating)
				{
					m_IsAnimating = true;
					interactionItemPanel.transform.DOMoveX(200, 1.0f).SetEase(Ease.InOutQuad).OnComplete(
						() => interactionItemPanel.transform.DOMoveX(200, 5.0f).SetEase(Ease.InOutQuad).OnComplete(
							() => interactionItemPanel.transform.DOMoveX(-200, 1.0f).SetEase(Ease.InOutQuad).OnComplete(
								() =>
								{
									interactionItemPanel.gameObject.SetActive(false);
									m_IsAnimating = false;
								}
							)
						)
					);
				}
			}

			interactionItemPanel.gameObject.SetActive(isOpenEvent);
		}

		public void ShowItemPanel(bool isOpenEvent, List<ItemStack> itemInfo, InteractionType interactionType)
		{
			if (isOpenEvent)
			{
				// interactionItemPanel.SetItem(itemInfo, interactionType);
				interactionItemPanel.FillInteractionPanel(interactionType);

				if (!m_IsAnimating)
				{
					m_IsAnimating = true;
					interactionItemPanel.transform.DOMoveX(200, 1.0f).SetEase(Ease.InOutQuad).OnComplete(
						() => interactionItemPanel.transform.DOMoveX(200, 5.0f).SetEase(Ease.InOutQuad).OnComplete(
							() => interactionItemPanel.transform.DOMoveX(-200, 1.0f).SetEase(Ease.InOutQuad).OnComplete(
								() =>
								{
									interactionItemPanel.gameObject.SetActive(false);
									m_IsAnimating = false;
								}
							)
						)
					);
				}
			}

			interactionItemPanel.gameObject.SetActive(isOpenEvent);
		}
	}
}
