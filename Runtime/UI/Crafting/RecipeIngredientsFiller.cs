using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Components.UI
{
	using Components;
	public class RecipeIngredientsFiller : MonoBehaviour
	{
		[SerializeField] private List<IngredientFiller> instantiatedGameObjects = new List<IngredientFiller>();

		public void FillIngredients(List<ItemStack> listOfIngredients, bool[] availabilityArray)
		{
			int maxCount = Mathf.Max(listOfIngredients.Count, instantiatedGameObjects.Count);

			for (int i = 0; i < maxCount; i++)
			{
				if (i < listOfIngredients.Count)
				{
					if (i >= listOfIngredients.Count)
					{
						// Do nothing, maximum ingredients for a recipe reached
						Debug.Log("Maximum ingredients reached");
					}
					else
					{
						// fill
						bool isAvailable = availabilityArray[i];
						instantiatedGameObjects[i].FillIngredient(listOfIngredients[i], isAvailable);

						instantiatedGameObjects[i].gameObject.SetActive(true);
					}

				}
				else if (i < instantiatedGameObjects.Count)
				{
					// Deactivate
					instantiatedGameObjects[i].gameObject.SetActive(false);
				}

			}


		}
	}
}
