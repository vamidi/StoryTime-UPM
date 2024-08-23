using UnityEngine;
using UnityEngine.UI;

namespace StoryTime.Domains.Game.UI
{
	[ExecuteInEditMode]
	public class ProgressBar : MonoBehaviour
	{
		public int minimum;
		public int maximum;
		public int current;
		public Image mask;
		public Image fill;
		public Color color;

		private void Update()
		{
			GetCurrentFill();
		}

		void GetCurrentFill()
		{
			float currentOffset = current - minimum;
			float maximumOffset = maximum - minimum;
			float fillAmount = currentOffset / maximumOffset;

			if(mask)
				mask.fillAmount = fillAmount;

			if(fill)
				fill.color = color;
		}
	}
}
