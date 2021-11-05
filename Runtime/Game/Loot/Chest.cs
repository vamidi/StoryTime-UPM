using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace StoryTime.Components
{
	using ScriptableObjects;

	public class Chest : MonoBehaviour
	{
		public string openAnimationTrigger = "OpenChest";
		public string closeAnimationTrigger = "CloseChest";

		[Header("Chest settings")]
		[SerializeField] private Animator animator;
		[SerializeField] private List<GameObject> chestEffects;
		[SerializeField, Tooltip("The items the player is going to receive")] private List<ItemStack> items;

		[SerializeField] private InventorySO playerInventory;
		[SerializeField, Tooltip("Whether the chest should be locked")] private bool locked = false;
		// private bool _hasObtained = false;

		// [Header("Listening to channels")]
		public UnityEvent<bool> lockChest;

		private void Start()
		{
			lockChest.AddListener(shouldBeLocked => locked = shouldBeLocked);
		}

		public List<ItemStack> Open()
		{
			List<ItemStack> returnItems = new List<ItemStack>();

			if (locked)
			{
				// TODO send message to the dialog manager to show we dont have unlocked the chest.
				return returnItems;
			}

			if (chestEffects.Count > 0)
			{
				foreach (var effect in chestEffects)
				{
					// THIS can be optimized
					/* var instantiatedEffect = */ Instantiate(effect, transform.position, transform.rotation);
				}
			}

			if (animator)
			{
				animator.SetTrigger(openAnimationTrigger);
			}

			// Check if we have enough space for all our items.
			if (playerInventory != null)
			{
				foreach (var item in items)
				{
					// if we don;t have space for that particularly item
					// close the chest
					// TODO send message to the dialog manager to show we dont have space.
					if (playerInventory.AvailabilityCheck(item) == false)
					{
						returnItems.Clear(); Close();
						break;
					}

					returnItems.Add(item);
				}

				// _hasObtained = returnItems.Count == items.Count;
			}

			return returnItems;
		}

		public void Close()
		{
			if (animator)
			{
				animator.SetTrigger(closeAnimationTrigger);
			}
		}
	}
}
