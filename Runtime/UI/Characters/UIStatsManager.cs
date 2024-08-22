using System.Collections.Generic;
using System.Collections.ObjectModel;
using StoryTime.Domains.Events.ScriptableObjects;
using TMPro;

using UnityEngine;

using StoryTime.Domains.Game.Characters;
using StoryTime.Domains.Game.Characters.ScriptableObjects;
using StoryTime.Domains.Game.Characters.ScriptableObjects.Events;

namespace  StoryTime.Components.UI
{
	using Components;

	public class UIStatsManager : MonoBehaviour
	{
		public CharacterSO SetCharacter
		{
			set => m_Character = value;
		}

		public int SelectedItemId
		{
			get => m_SelectedItemId;
			protected set => m_SelectedItemId = value;
		}

		public TextMeshProUGUI characterLevel;

		[Header("Player Settings")]
		[SerializeField] protected ProgressBar xpBar;

		[Header("Row Settings")]
		[SerializeField] protected List<StatListFiller> instantiatedRows = new List<StatListFiller>();
		// [SerializeField] protected StatListFiller statListFillerPrefab;
		[SerializeField] protected StatListFiller contentParent;
		// [SerializeField] protected GameObject contentParent;

		[Header("Listening on channels")]
		[SerializeField] protected VoidEventChannelSO onExpReceived;

		[Header("Broadcasting channels")]
		[SerializeField] protected StatEventChannelSO selectStatEvent;

		private CharacterSO m_Character;
		private int m_SelectedItemId = -1;

		private void Start()
		{
			if (onExpReceived != null)
				onExpReceived.OnEventRaised += Calculate;
		}

		protected virtual void HideItemInformation() { }

		public void FillStats()
		{
			if (m_Character)
			{
				characterLevel.text = m_Character.Level.ToString();

				if (m_Character.CharacterClass)
				{
					Calculate();
					FillItems(m_Character.CharacterClass.Stats);
				}
			}
		}

		private void FillItems(ReadOnlyCollection<CharacterStats> listItemsToShow)
		{
			if (instantiatedRows == null) instantiatedRows = new List<StatListFiller>();

			// We get the maximum number of all the rows.
			// this way know if we have more items then the instantiated rows and their capacity.
			contentParent.AddItems(listItemsToShow, false, selectStatEvent);

			// Hide information at first.
			HideItemInformation();

			// Unselect selected Item
			if (SelectedItemId >= 0)
			{
				UnselectItem(SelectedItemId);
				SelectedItemId = -1;
			}
		}

		private void ShowItemInformation(CharacterStats item)
		{
			// bool[] availabilityArray = m_RecipeInventory.IngredientsAvailability(currentInventory, item.Item.IngredientsList);
			// inspectorFiller.FillItemInspector(item, availabilityArray);
		}

		private void InspectItem(CharacterStats itemToInspect)
		{
			if (contentParent.SelectItem(SelectedItemId, itemToInspect, out var itemIndex))
			{
				// change Selected ID
				// TODO check if the indices also works for multiple rows.
				SelectedItemId = itemIndex;
				// Debug.Log($"UIInventoryManager: {SelectedItemId}");

				// show Information
				ShowItemInformation(itemToInspect);
			}
		}

		private void UnselectItem(int input)
		{
			// if (instantiatedRows.Count < input)
			// {
				contentParent.UnselectItem(input);
			// }
		}

		private void Calculate()
		{
			if (m_Character.CharacterClass)
			{
				//
				xpBar.minimum = 0;
				xpBar.current = m_Character.CurrentExp;
				xpBar.maximum = m_Character.MaxExp;
			}
		}
	}
}
