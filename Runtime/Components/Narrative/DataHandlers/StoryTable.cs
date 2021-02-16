using UnityEngine;

namespace DatabaseSync.Components
{
	using Binary;

	public class StoryTable : BaseTable<StorySO>
	{
		public new static StorySO ConvertRow(TableRow row, StorySO scriptableObject = null)
		{
			StorySO story = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<StorySO>();

			if (row.Fields.Count == 0)
			{
				return story;
			}

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					uint data = (uint) field.Value.Data;
					story.ID = data == uint.MaxValue - 1 ? uint.MaxValue : data;
				}

				if (field.Key.Equals("childId"))
				{
					uint data = (uint) field.Value.Data;
					story.ChildId = data == uint.MaxValue - 1 ? uint.MaxValue : data;
				}

				if (field.Key.Equals("description"))
				{
					story.Description = (string) field.Value.Data;
				}

				if (field.Key.Equals("title"))
				{
					story.Title = (string) field.Value.Data;
				}
			}

			return story;
		}
	}
}
