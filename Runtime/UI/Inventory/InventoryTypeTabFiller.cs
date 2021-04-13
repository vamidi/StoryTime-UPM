using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	using Events;
	public class InventoryTypeTabFiller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public InventoryTypeTabsFiller tabGroup;

		public Image bgImage;
		public Image bgInactiveImage;

		public Image image;
		public Image imgHover;
		public Image imgSelected;

		public LocalizeStringEvent tabName;

		public UnityEvent onTabSelected;
		public UnityEvent onTabDeselected;

		public virtual void FillTab(InventoryTabTypeSO tabType, bool isSelected, TabEventChannelSO changeTabEvent)
		{
			image.sprite = tabType.TabIcon;
			tabName.StringReference = tabType.TabName;
			// _actionButton.interactable = !isSelected;

			if (isSelected)
			{
				// image.color = _selectedIconColor;
			}
			else
			{
				// image.color = _deselectedIconColor;
			}

			if (onTabDeselected != null)
			{
				onTabSelected.RemoveAllListeners();
				onTabSelected.AddListener(() => changeTabEvent.RaiseEvent(tabType));
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			tabGroup.OnTabEnter(this);
			if(imgHover) imgHover.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			tabGroup.OnTabExit(this);
			if(imgHover) imgHover.gameObject.SetActive(false);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			tabGroup.OnTabSelected(this);
		}

		public void Select()
		{
			if(onTabSelected != null) onTabSelected.Invoke();
			if(imgSelected) imgSelected.gameObject.SetActive(true);
		}

		public void Deselect()
		{
			if(onTabDeselected != null) onTabDeselected.Invoke();
			if(imgSelected) imgSelected.gameObject.SetActive(false);
		}
	}
}
