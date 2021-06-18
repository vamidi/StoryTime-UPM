using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	using Components;
	public class InteractionStoryFiller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public StorySO Story => m_Story;

		[Header("References")]
		public StoryCategoryFiller categoryManager;

		[Header("Events")]
		public UnityEvent onRowSelected;
		public UnityEvent onRowDeselected;

		[SerializeField] private LocalizeStringEvent storyTitle;
		[SerializeField] private LocalizeStringEvent storySubTitle;

		private StorySO m_Story;

		float clicked = 0;
		float clicktime = 0;
		float clickdelay = 0.5f;

		public void Start()
		{
			categoryManager.Subscribe(this);
		}

		public void Set(StorySO story) => m_Story = story;

		public void OnPointerEnter(PointerEventData eventData)
		{
			categoryManager.OnRowEnter(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			categoryManager.OnRowExit(this);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			// Detecting double click
			clicked++;

			if (clicked == 1)
			{
				categoryManager.OnRowSelected(this);

				clicktime = Time.time;
			}

			if (clicked > 1 && Time.time - clicktime < clickdelay)
			{
				// Double click detected
				clicked = 0;
				clicktime = 0;
				categoryManager.OnRowDoubleClick(this);
			}
			else if (clicked > 2 || Time.time - clicktime > 1)
				clicked = 0;
		}

		public void FillInteractionPanel()
		{
			// TODO write a handler that return the text for the subtitle
			storyTitle.StringReference = m_Story.Title;
			if (m_Story.Tasks.Count > 0)
			{
				TaskSO task = m_Story.Tasks.FirstOrDefault(o => !o.IsDone) ?? m_Story.Tasks[0];
				storySubTitle.StringReference = task.Description;
			}
		}

		public void Select()
		{
			if(onRowSelected != null) onRowSelected.Invoke();
		}

		public void Deselect()
		{
			if(onRowDeselected != null) onRowDeselected.Invoke();
		}
	}
}
