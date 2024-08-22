using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Components.ScriptableObjects;
using StoryTime.Domains.ItemManagement.Loot;

namespace StoryTime.Editor.Wizards
{
	public class EnemyWizard : BaseWizard<EnemyWizard, EnemySO>
	{
		[Header("General Settings")]
		[SerializeField, Tooltip("Name of the enemy")] protected LocalizedString enemyName = new ();
		[SerializeField, Tooltip("Category of the enemy")] protected EnemySO.EnemyCategory category  = new ();
		[SerializeField, Tooltip("Amount of experience the enemy drops")] private uint exp = UInt32.MinValue;
		[SerializeField, Tooltip("Amount of money you get from an enemy")] private uint moneyReward = UInt32.MinValue;

		// [Header("Stats")]
		// private

		[Header("Drops")]
		[SerializeField, Tooltip("Amount of money you get from an enemy")] private List<DropItemStack> itemDrops = new ();

		public override void OnWizardUpdate()
		{
			helpString = "Create a new Character scriptable object";
			base.OnWizardUpdate();
		}

		protected override bool Validate()
		{
			if (enemyName.IsEmpty)
			{
				errorString = "Please fill in a name";
				return false;
			}

			errorString = "";
			return true;
		}

		protected override EnemySO Create(string location)
		{
			var enemy = CreateInstance<EnemySO>();
			enemy.EnemyName = enemyName;
			enemy.Category = category;
			enemy.Exp = exp;
			enemy.MoneyReward = moneyReward;

			// Add the equipments
			foreach (var drop in itemDrops)
			{
				enemy.AddItemDrop(drop);
			}

			return enemy;
		}
	}
}
