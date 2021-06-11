using UnityEngine;

namespace DatabaseSync
{
	using Components;
	using Input;
	using Events;

	public enum InteractionType
	{
		None = 0,
		PickUp,
		Cook,
		Craft,
		Talk,
		Navigate,
		Item,
		Object
	}

	public class InteractionManager : MonoBehaviour
	{
		protected UnityEngine.InputSystem.PlayerInput m_PlayerInput;

		private InteractionType _potentialInteraction;
		[HideInInspector] public InteractionType currentInteraction;

		[SerializeField] private BaseInputReader inputReader;

		//To store the object we are currently interacting with
		private GameObject _currentInteractableObject;

		[Header("Listening on channels")]
		// Check if the interaction ended
		[SerializeField] private VoidEventChannelSO onInteractionEnded;

		[SerializeField] private VoidEventChannelSO onStoryScreenOpenEvent;
		[SerializeField] private VoidEventChannelSO onStoryScreenCloseEvent;

		[Header("Inventory")]
		[SerializeField] private VoidEventChannelSO onInventoryScreenOpenEvent;
		[SerializeField] private VoidEventChannelSO onCloseInventoryScreenEvent;

		[Header("Crafting / Cooking")]
		[SerializeField] private VoidEventChannelSO onCloseCookingScreenEvent;


		[Header("BroadCasting on channels")]

		// Events for the different interaction types

		// event when there is an interaction with something.
		[SerializeField] private InteractionEventChannelSO onInteraction;

		[SerializeField] private ItemEventChannelSO onObjectPickUp;

		[SerializeField] private VoidEventChannelSO startCooking;
		[SerializeField] private CollectionEventChannelSO startCrafting;

		[SerializeField] private DialogueCharacterChannelSO startTalking;

		// UI events
		[SerializeField] private InteractionUIEventChannelSO toggleInteractionUI;

		private void OnEnable()
		{
			inputReader.interactEvent += OnInteractionButtonPress;
			onInteractionEnded.OnEventRaised += OnInteractionEnd;

			void EnableMenuInput()
			{
				// Change the action map
				inputReader.EnableMenuInput();
				m_PlayerInput.SwitchCurrentActionMap("Menus");
			}

			void DisableMenuInput()
			{
				// Change the action map
				inputReader.EnableGameplayInput();
				m_PlayerInput.SwitchCurrentActionMap("Gameplay");
			}

			if (onStoryScreenOpenEvent != null)
			{
				onStoryScreenOpenEvent.OnEventRaised += EnableMenuInput;
			}

			if (onStoryScreenCloseEvent != null)
			{
				onStoryScreenCloseEvent.OnEventRaised += DisableMenuInput;

			}

			if (onInventoryScreenOpenEvent != null)
			{
				onInventoryScreenOpenEvent.OnEventRaised += EnableMenuInput;
			}

			if (onCloseInventoryScreenEvent != null)
			{
				onCloseInventoryScreenEvent.OnEventRaised += DisableMenuInput;
			}

			if (onCloseCookingScreenEvent != null)
			{
				onCloseCookingScreenEvent.OnEventRaised += DisableMenuInput;
			}
		}

		private void OnDisable()
		{
			inputReader.interactEvent -= OnInteractionButtonPress;
			onInteractionEnded.OnEventRaised -= OnInteractionEnd;

			if (onStoryScreenOpenEvent != null)
			{
				onStoryScreenOpenEvent.OnEventRaised -= () =>
				{
					// Change the action map
					inputReader.EnableMenuInput();
					m_PlayerInput.SwitchCurrentActionMap("Menus");
				};
			}

			if (onStoryScreenCloseEvent != null)
			{
				onStoryScreenCloseEvent.OnEventRaised -= () =>
				{
					// Change the action map
					inputReader.EnableGameplayInput();
					m_PlayerInput.SwitchCurrentActionMap("Gameplay");
				};
			}
		}

		private void Awake()
		{
			m_PlayerInput = transform.root.GetComponent<UnityEngine.InputSystem.PlayerInput>();
		}

		//When the interaction ends, we still want to display the interaction UI if we are still in the trigger zone
		void OnInteractionEnd()
		{
			inputReader.EnableGameplayInput();
			m_PlayerInput.SwitchCurrentActionMap("Gameplay");

			switch (_potentialInteraction)
			{
				// we show it after cooking or talking, in case player want to interact again
				case InteractionType.Cook:
				case InteractionType.Talk:
					toggleInteractionUI.RaiseEvent(true, _potentialInteraction);
					Debug.Log("Display interaction UI");
					break;
			}
		}

		void OnInteractionButtonPress()
		{
			// remove interaction after press
			toggleInteractionUI.RaiseEvent(false, _potentialInteraction);

			switch (_potentialInteraction)
			{
				case InteractionType.None:
					return;
				case InteractionType.PickUp:
					if (_currentInteractableObject != null)
					{
						if (onObjectPickUp != null)
						{
							//raise an event with an item as parameter (to add object to inventory)
							ItemStack currentItem = _currentInteractableObject.GetComponent<CollectibleItem>().GetItem();
							onObjectPickUp.RaiseEvent(currentItem);

							Debug.Log("PickUp event raised");

							// set current interaction for state machine
							currentInteraction = InteractionType.PickUp;
						}
					}

					// destroy the GO
					Destroy(_currentInteractableObject);
					break;
				case InteractionType.Cook:
					if (startCooking != null)
					{
						startCooking.RaiseEvent();
						Debug.Log("Cooking event raised");
						//Change the action map
						inputReader.EnableMenuInput();
						m_PlayerInput.SwitchCurrentActionMap("Menus");

						//set current interaction for state machine
						currentInteraction = InteractionType.Cook;
					}
					break;
				case InteractionType.Craft:
					if(startCrafting != null)
					{
						// Debug.Log("Craft event raised");
						// raise an event with an actor as parameter
						// TODO check if the manager is set or that we should call the ui manager directly.
						var recipeManager = _currentInteractableObject.GetComponent<RecipeManager>();
						if (recipeManager)
						{
							recipeManager.InteractWithCharacter();

							//Change the action map
							inputReader.EnableMenuInput();
							m_PlayerInput.SwitchCurrentActionMap("Menus");
						}

						// Set current interaction for state machine
						currentInteraction = InteractionType.Craft;
					}
					break;
				case InteractionType.Talk:
					if (_currentInteractableObject != null)
					{
						if (startTalking != null)
						{
							// raise an event with an actor as parameter
							var revisionController = _currentInteractableObject.GetComponent<RevisionController>();
							revisionController.TurnToPlayer(transform.root.position);
							revisionController.InteractWithCharacter();

							// Change the action map
							inputReader.EnableDialogueInput();
							m_PlayerInput.SwitchCurrentActionMap("Dialogues");

							//set current interaction for state machine
							currentInteraction = InteractionType.Talk;
						}
					}

					break;
			}

			if(onInteraction != null)
				onInteraction.RaiseEvent(_currentInteractableObject, _potentialInteraction);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Pickable"))
			{
				_potentialInteraction = InteractionType.PickUp;
				// Debug.Log("I triggered a pickable object!");
				DisplayInteractionUI();
			}
			else if (other.CompareTag("CookingPot"))
			{
				_potentialInteraction = InteractionType.Cook;
				Debug.Log("I triggered a cooking pot!");
				DisplayInteractionUI();
			}
			else if (other.CompareTag("CraftingBench"))
			{
				_potentialInteraction = InteractionType.Craft;
				// Debug.Log("I triggered a crafting bench!");
				DisplayInteractionUI();
			}
			else if (other.CompareTag("NPC"))
			{
				_potentialInteraction = InteractionType.Talk;
				// Debug.Log("I triggered a npc!");
				DisplayInteractionUI();
			}

			_currentInteractableObject = other.gameObject;
		}

		private void DisplayInteractionUI()
		{
			// Raise event to display UI
			toggleInteractionUI.RaiseEvent(true, _potentialInteraction);
		}

		private void OnTriggerExit(Collider other)
		{
			ResetInteraction();
		}

		private void ResetInteraction()
		{
			_potentialInteraction = InteractionType.None;
			_currentInteractableObject = null;

			if (toggleInteractionUI != null)
				toggleInteractionUI.RaiseEvent(false, _potentialInteraction);
		}

		/*
		private void Confirm()
		{
			if (IsInteracting)
				return;

			// ReSharper disable once Unity.PreferNonAllocApi
			var colliders = Physics.OverlapSphere(transform.position, 50f, layerMask);
			float minDist = Mathf.Infinity;
			DefaultNPC closestNpc = null;
			foreach (var c in colliders)
			{
				var t = c.GetComponent<Transform>();
				float dist = Vector3.Distance(t.position, transform.position);
				if (dist < minDist)
				{
					closestNpc = c.GetComponent<DefaultNPC>();
				}
			}

			if (closestNpc != null)
			{
				// _dialogueManager.Interact(closestNpc.StoryComponent);
				IsInteracting = true;
			}
		}
		*/
	}
}
