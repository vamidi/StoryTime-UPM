using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync
{
	[CreateAssetMenu(fileName = "Interaction", menuName = "DatabaseSync/UI/Interaction", order = 51)]
	public class InteractionSO : ScriptableObject
	{
		// TODO add multi language support
		[Tooltip("The interaction name")]
		[SerializeField] private string interactionName = default;

		[Tooltip("The Interaction Type")]
		[SerializeField] private InteractionType interactionType = default;

		public string InteractionName => interactionName;
		public InteractionType InteractionType => interactionType;

	}
}
