using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync
{
	[CreateAssetMenu(fileName = "Interaction", menuName = "DatabaseSync/UI/Interaction", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class InteractionSO : ScriptableObject
	{
		public LocalizedString InteractionName
		{
			get => interactionName;
			set => interactionName = value;
		}

		public InteractionType InteractionType => interactionType;

		// TODO add multi language support
		[Tooltip("The interaction name")]
		[SerializeField] private LocalizedString interactionName;

		[Tooltip("The Interaction Type")]
		[SerializeField] private InteractionType interactionType;
	}
}
