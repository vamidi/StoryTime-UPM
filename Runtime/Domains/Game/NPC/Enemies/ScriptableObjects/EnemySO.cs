using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

using UnityEngine.Localization;

using StoryTime.Database.ScriptableObjects;
using StoryTime.Domains.ItemManagement.Loot;

namespace StoryTime.Domains.Game.NPC.Enemies.ScriptableObjects
{
	[CreateAssetMenu(fileName = "newEnemy", menuName = "StoryTime/Game/Enemy")]
	public partial class EnemySO : TableBehaviour
	{
		public LocalizedString EnemyName { get => enemyName; internal set => enemyName = value; }
		public EnemyCategory Category { get => category; internal set => category = value; }
		public uint Exp { get => exp; internal set => exp = value; }
		public uint MoneyReward { get => moneyReward; internal set => moneyReward = value; }
		public ReadOnlyCollection<DropItemStack> ItemDrops => itemDrops.AsReadOnly();

		[Header("General Settings")]
		[SerializeField, Tooltip("Name of the enemy")] private LocalizedString enemyName;
		[SerializeField] private EnemyCategory category = new();
		[SerializeField, Tooltip("Amount of experience the enemy drops")] private uint exp = UInt32.MinValue;
		[SerializeField, Tooltip("Amount of money you get from an enemy")] private uint moneyReward = UInt32.MinValue;

		// [Header("Stats")]
		// private

		[Header("Drops")]
		[SerializeField, Tooltip("Amount of money you get from an enemy")] private List<DropItemStack> itemDrops = new ();

		public EnemySO() : base("enemies", "name") { }

		[Serializable]
		public class EnemyCategory
		{
			public uint categoryId = UInt32.MinValue;
			public LocalizedString categoryName = new ();
		}

		public void AddItemDrop(DropItemStack stack)
		{
			itemDrops.Add(stack);
		}
	}
}
