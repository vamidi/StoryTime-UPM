using UnityEngine;

namespace DatabaseSync
{
	using Components;
	using Input;
	using ResourceManagement.Util;

	public enum InteractionType
	{
		None = 0,
		PickUp,
		Cook,
		Talk
	};

	public class InteractionManager : MonoBehaviour
	{
		protected UnityEngine.InputSystem.PlayerInput m_PlayerInput;

		private InteractionType _potentialInteraction;
		[HideInInspector] public InteractionType currentInteraction;

		[SerializeField] private InputReader inputReader = default;

		//To store the object we are currently interacting with
		private GameObject _currentInteractableObject = null;

		//Events for the different interaction types
		// [Header("Broadcasting on")] [SerializeField]
		// private ItemEventChannelSo _onObjectPickUp = default;

		[SerializeField] private VoidEventChannelSO onCookingStart = default;

		[SerializeField] private DialogueActorChannelSO startTalking = default;

		//UI event
		[SerializeField] private InteractionUIEventChannelSO toggleInteractionUI = default;

		[Header("Listening to")]
		// Check if the interaction ended
		[SerializeField]
		private VoidEventChannelSO onInteractionEnded = default;

		private void OnEnable()
		{
			inputReader.interactEvent += OnInteractionButtonPress;
			onInteractionEnded.OnEventRaised += OnInteractionEnd;
		}

		private void OnDisable()
		{
			inputReader.interactEvent -= OnInteractionButtonPress;
			onInteractionEnded.OnEventRaised -= OnInteractionEnd;
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
				//we show it after cooking or talking, in case player want to interact again
				case InteractionType.Cook:
				case InteractionType.Talk:
					toggleInteractionUI.RaiseEvent(true, _potentialInteraction);
					Debug.Log("Display interaction UI");
					break;
				default:
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
						/*
						if (_onObjectPickUp != null)
						{
							//raise an event with an item as parameter (to add object to inventory)
							Item currentItem = _currentInteractableObject.GetComponent<CollectibleItem>().GetItem();
							_onObjectPickUp.RaiseEvent(currentItem);
							Debug.Log("PickUp event raised");
							//set current interaction for state machine
							currentInteraction = InteractionType.PickUp;
						}
						*/
					}

					//destroy the GO
					Destroy(_currentInteractableObject);
					break;
				case InteractionType.Cook:
					if (onCookingStart != null)
					{
						onCookingStart.RaiseEvent();
						Debug.Log("Cooking event raised");
						//Change the action map
						inputReader.EnableMenuInput();
						m_PlayerInput.SwitchCurrentActionMap("Menus");

						//set current interaction for state machine
						currentInteraction = InteractionType.Cook;
					}

					break;
				case InteractionType.Talk:
					if (_currentInteractableObject != null)
					{
						if (startTalking != null)
						{
							// raise an event with an actor as parameter
							_currentInteractableObject.GetComponent<RevisionController>().InteractWithCharacter();

							// Change the action map
							inputReader.EnableDialogueInput();
							m_PlayerInput.SwitchCurrentActionMap("Dialogues");

							//set current interaction for state machine
							currentInteraction = InteractionType.Talk;
						}
					}

					break;
				default:
					break;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Pickable"))
			{
				_potentialInteraction = InteractionType.PickUp;
				Debug.Log("I triggered a pickable object!");
				DisplayInteractionUI();
			}
			else if (other.CompareTag("CookingPot"))
			{
				_potentialInteraction = InteractionType.Cook;
				Debug.Log("I triggered a cooking pot!");
				DisplayInteractionUI();
			}
			else if (other.CompareTag("NPC"))
			{
				_potentialInteraction = InteractionType.Talk;
				DisplayInteractionUI();
			}

			_currentInteractableObject = other.gameObject;
		}

		private void DisplayInteractionUI()
		{
			//Raise event to display UI
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
