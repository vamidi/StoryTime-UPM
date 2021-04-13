using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	using Events;
	using Components;

	public abstract class ItemBaseFiller<TStack, TItem> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
		where TItem : ItemSO
		where TStack: ItemBaseStack<TItem>
	{
		[HideInInspector] public TStack currentItem;

		[SerializeField] protected LocalizeStringEvent itemName;

		[SerializeField] protected Image itemPreviewImage;
		[SerializeField] protected Image bgImage;
		[SerializeField] protected Image imgHover;
		[SerializeField] protected Image imgSelected;

		[SerializeField] protected LocalizeSpriteEvent bgLocalizedImage;

		protected ItemEventChannelSO m_SelectItemEvent;

		public virtual void SetItem(TStack itemStack, bool isSelected, ItemEventChannelSO selectItemEvent)
		{
			itemPreviewImage.gameObject.SetActive(true);
			itemName.gameObject.SetActive(true);
			bgImage.gameObject.SetActive(true);
			imgHover.gameObject.SetActive(true);
			imgSelected.gameObject.SetActive(true);

			currentItem = itemStack;
			m_SelectItemEvent = selectItemEvent;

			imgSelected.gameObject.SetActive(isSelected);
			if (itemStack.Item.IsLocalized)
			{
				bgLocalizedImage.enabled = true;
				bgLocalizedImage.AssetReference = itemStack.Item.LocalizePreviewImage;
			}
			else
			{
				bgLocalizedImage.enabled = false;
				itemPreviewImage.sprite = itemStack.Item.PreviewImage;
			}

			bgImage.color = itemStack.Item.ItemType.TypeColor;
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			imgHover.gameObject.SetActive(true);
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			imgHover.gameObject.SetActive(false);
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			SelectItem();

			if (m_SelectItemEvent != null)
			{
				if(currentItem is ItemStack item)
					m_SelectItemEvent.RaiseEvent(item);
				else if(currentItem is ItemRecipeStack stack)
					m_SelectItemEvent.RaiseEvent(stack);
				else Debug.LogWarning("Event cant be called because item is unknown. overload to supper item type.");
			}
		}

		public virtual void SetInactiveItem()
		{
			itemPreviewImage.gameObject.SetActive(false);
			itemName.gameObject.SetActive(false);
			bgImage.gameObject.SetActive(false);
			imgHover.gameObject.SetActive(false);
			imgSelected.gameObject.SetActive(false);
			OnPointerExit(null);
		}

		public virtual void SelectItem()
		{
			imgSelected.gameObject.SetActive(true);
		}

		public virtual void UnSelectItem()
		{
			imgSelected.gameObject.SetActive(false);
		}
	}
}
