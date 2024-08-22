
using UnityEngine;

using StoryTime.Database.Binary;

namespace StoryTime.Domains.Narrative.Tasks.Services
{
	using ScriptableObjects;

	public class TaskService
	{
		public TaskSO ConvertRow(TableRow row, TaskSO scriptableObject = null)
		{
			TaskSO task = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<TaskSO>();

			if (row.Fields.Count == 0)
			{
				return task;
			}

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					task.ID = uint.Parse(field.Value.Data);
				}

				if (field.Key.Equals("nextId"))
				{
					string data = field.Value.Data;
					task.NextId = data;
				}

				if (field.Key.Equals("description"))
				{
					// task.Description = (string) field.Value.Data;
				}

				if (field.Key.Equals("hidden"))
				{
					task.Hidden = (bool)field.Value.Data;
				}

				if (field.Key.Equals("npc"))
				{
					string data = field.Value.Data;
					task.Npc = data;
				}

				if (field.Key.Equals("parentId"))
				{
					string data = field.Value.Data;
					// TODO search parent based on id
					// task.ParentId = data;
					// Debug.Log($"parentId {task.ParentId}");
				}

				if (field.Key.Equals("requiredCount"))
				{
					task.RequiredCount = (uint)field.Value.Data;
				}

				if (field.Key.Equals("typeId"))
				{
					task.Type = (TaskCompletionType)field.Value.Data;
				}

				if (field.Key.Equals("enemyCategory"))
				{
					// task.enemyCategory = new EnemySO.EnemyCategory
					// {
						// categoryId = (uint)field.Value.Data
					// };
				}
			}

			return task;
		}
	}
}