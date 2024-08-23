using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StoryTime.Domains.Narrative.UI.Stories
{
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	
	[RequireComponent(typeof(Image))]
	public class StoryCategoryTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public Sprite categoryImage;
		public UIStoryManager manager;
		public StoryType type;

		[Header("Events")]
		public UnityEvent onTabSelected;

		private Image m_Image;

		public void Start()
		{
			m_Image = GetComponent<Image>();
			m_Image.sprite = categoryImage;
		}

		public void Select()
		{
			if(onTabSelected != null) onTabSelected.Invoke();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			// Do something when we hover over it.
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			// Do something when we hover out.
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			manager.OnTabSelected(this);
		}
	}
}
