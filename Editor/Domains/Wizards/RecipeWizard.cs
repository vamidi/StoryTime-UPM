
namespace StoryTime.Editor.Domains.Wizards
{
	using StoryTime.Domains.ItemManagement.Equipment.ScriptableObjects;

	/// <summary>
	/// The recipe wizard is there to create Recipes or
	/// craftables scriptable objects.
	/// </summary>
	public class RecipeWizard : BaseWizard<RecipeWizard, EquipmentSO>
	{
		public void OnWizardCreate()
		{

		}

		public void OnWizardUpdate()
		{

		}

		public void OnWizardOtherButton()
		{

		}

		protected override bool Validate()
		{
			throw new System.NotImplementedException();
		}

		protected override EquipmentSO Create(string location)
		{
			throw new System.NotImplementedException();
		}
	}
}
