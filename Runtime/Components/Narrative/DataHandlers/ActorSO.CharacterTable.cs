namespace DatabaseSync.Components
{
	using Binary;

	/// <summary>
	/// Dialogue Holder
	/// these are the base variables that we need to set
	/// up the dialogue system
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public partial class ActorSO
	{
		public class CharacterTable : BaseTable<ActorSO>
		{
			public new static ActorSO ConvertRow(TableRow row, ActorSO scriptableObject)
			{
				ActorSO actor = scriptableObject ? scriptableObject : CreateInstance<ActorSO>();

				if (row.Fields.Count == 0)
				{
					return actor;
				}

				DatabaseConfig config = TableBinary.Fetch();
				if (config != null)
				{
					actor.ID = row.RowId;
					var entryId = (actor.ID + 1).ToString();

					if(!actor.actorName.IsEmpty) actor.actorName.TableEntryReference = entryId;
				}

				foreach (var field in row.Fields)
				{
					if (field.Key.Equals("id"))
					{
						actor.ID = uint.Parse(field.Value.Data);
					}
				}

				return actor;
			}
		}
	}
}
