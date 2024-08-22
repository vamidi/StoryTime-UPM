using UnityEngine;

using StoryTime.Domains.Game.Characters;
namespace StoryTime.Components.UI
{
	public class StatInspectorFiller: MonoBehaviour
	{
		[SerializeField] private StatItemFiller statsFiller = default;

		public void FillStatInspector(CharacterStats itemToInspect)
		{
			// statsFiller.FillStats(itemToInspect);
			// statsFiller.gameObject.SetActive(true);
		}
	}
}
