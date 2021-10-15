using DatabaseSync.Game;
using TMPro;

using UnityEngine;

namespace DatabaseSync.UI
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
