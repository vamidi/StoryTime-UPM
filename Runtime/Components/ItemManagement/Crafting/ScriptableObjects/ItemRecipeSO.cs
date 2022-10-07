using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
#endif

using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Components.ScriptableObjects
{
	using FirebaseService.Database;

	[CreateAssetMenu(fileName = "itemRecipe", menuName = "StoryTime/Item Management/Item Recipe", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class ItemRecipeSO : ItemSO
	{
		public List<ItemStack> IngredientsList => ingredientsList;
		public ItemSO ResultingDish => resultingDish;

		public uint ChildId
		{
			get => childId;
			set => childId = value;
		}

		[SerializeField, HideInInspector, Tooltip("Child id, which is the reference to the itemId")] protected UInt32 childId;

		// TODO add Recipe functionality
		[SerializeField, Tooltip("The list of the ingredients necessary to the recipe")]
		private List<ItemStack> ingredientsList = new List<ItemStack>();

		[SerializeField, Tooltip("The resulting dish to the recipe")] private ItemSO resultingDish;

		ItemRecipeSO() : base("shopCraftables", "name", "childId", UInt32.MaxValue, "items") { }

		public override void OnEnable()
		{
			base.OnEnable();
			Initialize();
		}

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();

			Initialize();
			resultingDish = this;
		}

		protected override void Initialize()
		{
#if UNITY_EDITOR
			if (ID != UInt32.MaxValue)
			{
				var entryId = (childId + 1).ToString();
				collection = LocalizationEditorSettings.GetStringTableCollection("Item Names");
				if (collection != null)
					itemName = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables for Items");

				var descriptionCollection = overrideDescriptionTable ? itemDescriptionCollection : LocalizationEditorSettings.GetStringTableCollection("Item Descriptions");
				if (descriptionCollection != null)
					description = new LocalizedString { TableReference = descriptionCollection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables for Items");

				var field = TableDatabase.Get.GetField(Name, "data", ID);
				if (field != null)
				{
#if UNITY_EDITOR
					// if we want to remove the exising list.
					// if (ingredientsList.Count > 0 && EditorUtility.DisplayDialog("Remove existing ingredients",
						// "Do you want to remove existing list?\n\nYou cannot undo this action.", "Yes", "No"))
					// {
						// ingredientsList.Clear();
					// }
#endif
					ParseNodeData(this, (JObject) field.Data);
				}
			}

			resultingDish = this;
#endif
		}


		/// <summary>
		///
		/// </summary>
		private static void ParseNodeData(ItemRecipeSO recipe, JObject jObject)
		{
			if (jObject["nodes"] == null)
				return;

			JObject nodes = jObject["nodes"].ToObject<JObject>();

			// Retrieve the first node. because that is the start node.
			// if not debug show error.
			var nodeToken = nodes.First.Value<JProperty>();

			var node = nodeToken.Value.ToObject<JObject>();
			if (node["name"].ToObject<string>().ToLower() != "itemmaster")
			{
				Debug.LogWarning("First Node is not the master node");
				return;
			}

			if (node["data"] == null)
				return;

			var data = node["data"].ToObject<JObject>();

			// check what is inside the node
			if (data != null)
			{
				// get the inputs
				var inputs = node["inputs"].ToObject<JObject>();

				// loop through the inputs
				foreach (var inputToken in inputs)
				{
					var input = inputToken.Value.ToObject<JObject>();
					var connections = input["connections"].ToArray();
					var emptyObj = new JObject();

					if (inputToken.Key.Contains("item") && connections.Length > 0)
					{
						foreach (var con in connections)
						{
							// grab the other node id.
							var nodeId = con["node"]?.ToObject<int>().ToString() ?? String.Empty;
							// grab the other node object.
							var otherNode = nodeId != String.Empty ? nodes[nodeId].Value<JObject>() : emptyObj;
							// grab the data from the other node.
							var otherData = otherNode["data"]?.ToObject<JObject>() ?? emptyObj;

							// fetch the item id
							var itemId = otherData["itemId"]?.ToObject<uint>() ?? UInt32.MaxValue;
							if (itemId != UInt32.MaxValue)
							{
								var link = TableDatabase.Get.FindLink("shopCraftConditions", "childId", itemId);

								if (link.Item1 != UInt32.MaxValue && link.Item2.Fields.Count > 0)
								{
									int amount = 1;
									var amountField =  link.Item2.Find("amount");
									if (amountField != null)
										amount = (int) amountField.Data;

									ItemSO item = null;

									// find the asset if not create it.
									string[] guids = AssetDatabase.FindAssets(" t:itemSO", null);
									foreach (string guid in guids)
									{
										item = AssetDatabase.LoadAssetAtPath<ItemSO>(AssetDatabase.GUIDToAssetPath(guid));
										if (item.ID == itemId)
											break;

										item = null;
									}

									//  we haven't found the item let us create it.
									if (item == null)
									{
										ItemSO asset = CreateInstance<ItemSO>();

										var destination = "./ingredients";
										if (!Directory.Exists(destination))
											Directory.CreateDirectory(destination);

										AssetDatabase.CreateAsset(asset, destination);
										AssetDatabase.SaveAssets();
									}

									if(recipe.ingredientsList.Find(i => i.Item.ID == itemId) == null)
										recipe.ingredientsList.Add(new ItemStack(item, amount));
								}
							}
						}
					}
				}
			}
		}
	}
}
