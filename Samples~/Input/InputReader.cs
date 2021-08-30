using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace DatabaseSync.Input
{
	[CreateAssetMenu(fileName = "InputReader", menuName = "DatabaseSync/Game/Input Reader")]
	public class InputReader : BaseInputReader, GameInput.IGameplayActions, GameInput.IDialoguesActions,
		GameInput.IMenusActions
	{
		// Gameplay
		public override event UnityAction jumpEvent = delegate { };
		public override event UnityAction jumpCanceledEvent = delegate { };
		public override event UnityAction fireEvent = delegate { };
		public override event UnityAction aimEvent = delegate { };
		public override event UnityAction interactEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
		public override event UnityAction openInventoryEvent = delegate { }; // Used to bring up the inventory
		public override event UnityAction pauseEvent = delegate { };
		public override event UnityAction<Vector2> moveEvent = delegate { };
		public override event UnityAction<Vector2, bool> cameraMoveEvent = delegate { };
		public override event UnityAction enableMouseControlCameraEvent = delegate { };
		public override event UnityAction disableMouseControlCameraEvent = delegate { };
		public override event UnityAction startAbility1 = delegate { };
		public override event UnityAction stopAbility1 = delegate { };
 
		// Shared between menus and dialogues
		public override event UnityAction moveSelectionEvent = delegate { };
 
		// Dialogues
		public override event UnityAction advanceDialogueEvent = delegate { };
 
		// Menus
		public override event UnityAction menuMouseMoveEvent = delegate { };
		public override event UnityAction menuConfirmEvent = delegate { };
		public override event UnityAction menuCancelEvent = delegate { };
		public override event UnityAction menuUnpauseEvent = delegate { };

		private GameInput gameInput;

		protected override void OnEnable()
		{
			if (gameInput == null)
			{
				gameInput = new GameInput();
				gameInput.Menus.SetCallbacks(this);
				gameInput.Gameplay.SetCallbacks(this);
				gameInput.Dialogues.SetCallbacks(this);
			}

			EnableGameplayInput();
		}

		protected override void OnDisable()
		{
			DisableAllInput();
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				fireEvent.Invoke();
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			if(context.phase == InputActionPhase.Performed)
				aimEvent.Invoke();
		}

		public void OnOpenInventory(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				openInventoryEvent.Invoke();
		}

		public void OnInteract(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				interactEvent.Invoke();
		}

		public void Interact()
		{
			interactEvent.Invoke();
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				jumpEvent.Invoke();

			if (context.phase == InputActionPhase.Canceled)
				jumpCanceledEvent.Invoke();
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			moveEvent.Invoke(context.ReadValue<Vector2>());
		}

		public void OnAbility1(InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Performed:
					startAbility1.Invoke();
					break;
				case InputActionPhase.Canceled:
					stopAbility1.Invoke();
					break;
			}
		}

		public void OnPause(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				pauseEvent.Invoke();
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			cameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
		}

		public void OnMouseControlCamera(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				enableMouseControlCameraEvent.Invoke();

			if (context.phase == InputActionPhase.Canceled)
				disableMouseControlCameraEvent.Invoke();
		}

		private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

		public void OnMoveSelection(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				moveSelectionEvent.Invoke();
		}

		public void OnAdvanceDialogue(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				advanceDialogueEvent.Invoke();
		}

		public void OnConfirm(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				menuConfirmEvent.Invoke();
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				menuCancelEvent.Invoke();
		}

		public void OnMouseMove(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				menuMouseMoveEvent.Invoke();
		}

		public void OnUnpause(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				menuUnpauseEvent.Invoke                                                                                                                                                                                                                                                                                                                                                                         ();
		}

		public override void EnableDialogueInput()
		{
			gameInput.Menus.Disable();
			gameInput.Gameplay.Disable();

			gameInput.Dialogues.Enable();
		}

		public override void EnableGameplayInput()
		{
			gameInput.Menus.Disable();
			gameInput.Dialogues.Disable();

			gameInput.Gameplay.Enable();
		}

		public override void EnableMenuInput()
		{
			gameInput.Dialogues.Disable();
			gameInput.Gameplay.Disable();

			gameInput.Menus.Enable();
		}

		public override void DisableAllInput()
		{
			gameInput.Gameplay.Disable();
			gameInput.Menus.Disable();
			gameInput.Dialogues.Disable();
		}
	}
}
