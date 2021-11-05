using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

namespace StoryTime.Components.UI
{
	using Components.ScriptableObjects;

	/// <summary>
	/// A storyFiller is
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class StoryCategoryFiller : MonoBehaviour
	{
		[Header("Presentation")]
		// public Sprite rowIdle;
		// public Sprite rowHover;
		// public Sprite rowActive;
		public Image background;

		[Header("References")]
		public InteractionStoryFiller storyRowPrefab;
		public InteractionStoryFiller selectedRow;
		public UIStoryManager manager;

		public StoryType Category => m_Category;

		[Header("Broadcasting channels")]
		[SerializeField] private Events.ScriptableObjects.VoidEventChannelSO closeStoryScreenEvent;
		[SerializeField] private Events.ScriptableObjects.StoryEventChannelSO onStorySelectEvent;

		[SerializeField] private LocalizedString categoryTitle;

		private readonly List<InteractionStoryFiller> m_RowItems = new List<InteractionStoryFiller>();

		private StoryType m_Category;

		public void Start()
		{
			manager.Subscribe(this);
		}

		public void Add(StorySO story)
		{
			InteractionStoryFiller row = Instantiate(storyRowPrefab, Vector3.zero, Quaternion.identity);
			row.categoryManager = this;

			selectedRow = row;

			var trans = row.transform;
			trans.SetParent(transform);
			trans.localScale = Vector3.one;

			// fill the in the item in the story log.
			row.Set(story);
			row.FillInteractionPanel();
			m_RowItems.Add(row);
		}

		public void Subscribe(InteractionStoryFiller row)
		{
			m_RowItems.Add(row);
		}

		public void OnRowEnter(InteractionStoryFiller row)
		{
			ResetRows();
			// if(selectedRow == null || row != selectedRow)
			// row.background.sprite = rowHover;
		}

		public void OnRowExit(InteractionStoryFiller row)
		{
			ResetRows();
		}

		public void OnRowSelected(InteractionStoryFiller row)
		{
			if (selectedRow != null) selectedRow.Deselect();

			selectedRow = row;
			selectedRow.Select();

			ResetRows();
			// row.background.sprite = rowActive;
			int index = row.transform.GetSiblingIndex();
			var rowItem = index < m_RowItems.Count ? m_RowItems[index] : null;

			if (rowItem)
			{
				rowItem.Select();
				manager.OnCategoryRowSelected(rowItem);
			}
		}

		public void OnRowDoubleClick(InteractionStoryFiller row)
		{
			if (closeStoryScreenEvent != null)
			{
				closeStoryScreenEvent.RaiseEvent();
				if(onStorySelectEvent != null) onStorySelectEvent.RaiseEvent(row.Story);
			}
		}

		public void ResetRows()
		{
			// foreach (var row in m_RowItems)
			// {
			// if (selectedRow != null && row == selectedRow) continue;
			// row.background.sprite = rowIdle;
			// }
		}

		public void FillCategory(KeyValuePair<StoryType, List<StorySO>> item)
		{
			m_Category = item.Key;

			FillInteractionPanel();

			foreach (var story in item.Value)
			{
				if( m_Category == StoryType.All || story.TypeId == m_Category)
					Add(story);
			}
		}

		public void FillInteractionPanel()
		{
			// TODO write a handler that return the text for the subtitle
			string reference = m_Category.ToString();
			// categoryTitle.StringReference = reference;
		}
	}
}
