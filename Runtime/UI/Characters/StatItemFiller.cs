using System.Globalization;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;

using TMPro;

namespace StoryTime.Components.UI
{
	using Events.ScriptableObjects;

	public class StatItemFiller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[HideInInspector] public CharacterStats currentStats;

		[SerializeField] protected LocalizeStringEvent statName;
		[SerializeField] protected TextMeshProUGUI statValue;

		[SerializeField] protected Image itemPreviewImage;
		[SerializeField] protected Image bgImage;
		[SerializeField] protected Image imgHover;
		[SerializeField] protected Image imgSelected;

		[SerializeField] protected LocalizeSpriteEvent bgLocalizedImage;

		protected StatEventChannelSO m_SelectItemEvent;

		public virtual void SetStat(CharacterStats stats, bool isSelected, StatEventChannelSO selectItemEvent)
		{
			statName.gameObject.SetActive(true);

			if(itemPreviewImage)
				itemPreviewImage.gameObject.SetActive(true);

			if(bgImage)
				bgImage.gameObject.SetActive(true);

			if(imgHover)
				imgHover.gameObject.SetActive(true);

			if(imgSelected)
				imgSelected.gameObject.SetActive(true);

			if(imgSelected)
				imgSelected.gameObject.SetActive(isSelected);

			currentStats = stats;

			m_SelectItemEvent = selectItemEvent;

			if (stats.IsLocalized)
			{
				if (bgLocalizedImage)
				{
					bgLocalizedImage.enabled = true;
					bgLocalizedImage.AssetReference = stats.LocalizePreviewImage;
				}
			}
			else
			{
				if (bgLocalizedImage)
					bgLocalizedImage.enabled = false;

				if(itemPreviewImage)
					itemPreviewImage.sprite = stats.PreviewImage;
			}
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			if(imgHover)
				imgHover.gameObject.SetActive(true);
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			if(imgHover)
				imgHover.gameObject.SetActive(false);
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			SelectItem();

			if (m_SelectItemEvent != null)
			{
				m_SelectItemEvent.RaiseEvent(currentStats);
			}
		}

		public virtual void SetInactiveItem()
		{
			if(itemPreviewImage)
				itemPreviewImage.gameObject.SetActive(false);
			statName.gameObject.SetActive(false);

			if(bgImage)
				bgImage.gameObject.SetActive(false);

			if(imgHover)
				imgHover.gameObject.SetActive(false);

			if(imgSelected)
				imgSelected.gameObject.SetActive(false);

			OnPointerExit(null);
		}

		public virtual void SelectItem()
		{
			if(imgSelected)
				imgSelected.gameObject.SetActive(true);
		}

		public virtual void UnSelectItem()
		{
			if(imgSelected)
				imgSelected.gameObject.SetActive(false);
		}

		public void FillStats()
		{
			statName.StringReference = currentStats.StatName;
			statValue.text = currentStats.Value.ToString(CultureInfo.InvariantCulture);
		}
	}
}
