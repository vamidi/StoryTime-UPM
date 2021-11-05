using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StoryTime.Components.UI
{
	[RequireComponent(typeof(Image))]
	public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public TabGroup tabGroup;
		public Image background;

		public UnityEvent onTabSelected;
		public UnityEvent onTabDeselected;

		private void Start()
		{
			background = GetComponent<Image>();
			tabGroup.Subscribe(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			tabGroup.OnTabEnter(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			tabGroup.OnTabExit(this);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			tabGroup.OnTabSelected(this);
		}

		public void Select()
		{
			if(onTabSelected != null) onTabSelected.Invoke();
		}

		public void Deselect()
		{
			if(onTabDeselected != null) onTabDeselected.Invoke();
		}
	}
}
