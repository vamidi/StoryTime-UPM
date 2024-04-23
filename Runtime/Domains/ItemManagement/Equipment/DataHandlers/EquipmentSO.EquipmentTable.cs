using StoryTime.Database.Binary;

namespace StoryTime.Components.ScriptableObjects
{
	public partial class EquipmentSO : IBaseTable<EquipmentSO>
	{
		public EquipmentSO ConvertRow(TableRow row, EquipmentSO scriptableObject = null)
		{
			EquipmentSO item = scriptableObject ? scriptableObject : CreateInstance<EquipmentSO>();

			return base.ConvertRow(row, item) as EquipmentSO;
		}
	}
}
