using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	using Database.Binary;

	public partial class EnemySO : IBaseTable<EnemySO>
	{
		public EnemySO ConvertRow(TableRow row, EnemySO scriptableObject = null)
		{
			EnemySO enemy = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<EnemySO>();

			if (row.Fields.Count == 0)
			{
				return enemy;
			}

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					enemy.ID = uint.Parse(field.Value.Data);
				}

				if (field.Key.Equals("category"))
				{
					enemy.Category = (uint) field.Value.Data;
				}

				if (field.Key.Equals("name"))
				{
					enemy.EnemyName = (string) field.Value.Data;
				}
			}

			return enemy;
		}
	}
}