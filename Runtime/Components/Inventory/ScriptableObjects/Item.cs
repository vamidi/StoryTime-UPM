using UnityEngine;

namespace DatabaseSync
{
	[CreateAssetMenu(fileName = "Item", menuName = "DatabaseSync/Inventory/Item", order = 51)]
	public class Item : ScriptableObject
	{
		[Tooltip("The name of the item")]
		[SerializeField] private new string name = default;

		[Tooltip("A preview image for the item")]
		[SerializeField]
		private Sprite previewImage = default;

		[Tooltip("A description of the item")]
		[SerializeField]
		private string description = default;

		[Tooltip("The type of item")]
		[SerializeField]
		private ItemType itemType = default;

		[Tooltip("A prefab reference for the model of the item")]
		[SerializeField]
		private GameObject prefab = default;

		// TODO add Recipe functionality
		// [Tooltip("The list of the ingredients necessary to the recipe")]
		// [SerializeField]
		// private List<ItemStack> _ingredientsList = new List<ItemStack>();

		[Tooltip("The resulting dish to the recipe")]
		[SerializeField]
		private Item resultingDish = default;

		public string Name => name;
		public Sprite PreviewImage => previewImage;
		public string Description => description;
		public ItemType ItemType => itemType;
		public GameObject Prefab => prefab;
		// TODO add Recipe functionality
		// public List<ItemStack> IngredientsList => ingredientsList;
		public Item ResultingDish => resultingDish;

	}
}
