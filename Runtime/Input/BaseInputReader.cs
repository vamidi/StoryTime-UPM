using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace StoryTime.Input.ScriptableObjects
{
	public abstract class BaseInputReader : ScriptableObject
	{
		// Gameplay
		public abstract event UnityAction jumpEvent;
		public abstract event UnityAction jumpCanceledEvent;
		public abstract event UnityAction pauseEvent;
		public abstract event UnityAction<Vector2> moveEvent;
		public abstract event UnityAction<Vector2, bool> cameraMoveEvent;
		public abstract event UnityAction enableMouseControlCameraEvent;
		public abstract event UnityAction disableMouseControlCameraEvent;
		public abstract event UnityAction aimEvent;

		public abstract event UnityAction fireEvent;

		// Shared between menus and dialogues
		public abstract event UnityAction openInventoryEvent; // Used to bring up the inventory
		public abstract event UnityAction interactEvent; // Used to talk, pickup objects, interact with tools like the cooking cauldron
		public abstract event UnityAction advanceDialogueEvent;
		public abstract event UnityAction moveSelectionEvent;

		public abstract event UnityAction startAbility1;
		public abstract event UnityAction stopAbility1;


		public abstract event UnityAction menuMouseMoveEvent;
		public abstract event UnityAction menuConfirmEvent;
		public abstract event UnityAction menuCancelEvent;
		public abstract event UnityAction menuUnpauseEvent;

		public event UnityAction toggleDebugEvent = delegate { };
		public event UnityAction returnEvent = delegate { };

		public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

		protected virtual void OnEnable()
		{
			EnableGameplayInput();
		}

		protected virtual void OnDisable()
		{
			DisableAllInput();
		}

		public virtual void OnToggleDebug(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				toggleDebugEvent.Invoke();
		}

		public virtual void OnReturn(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				returnEvent.Invoke();
		}

		public abstract void EnableDialogueInput();
		public abstract void EnableGameplayInput();
		public abstract void EnableMenuInput();
		public abstract void DisableAllInput();
	}
}
