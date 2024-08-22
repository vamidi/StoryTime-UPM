using System.Linq;
using System.Collections.Generic;

namespace StoryTime.Domains.ItemManagement.Extensions
{
    using Inventory;
    using Inventory.ScriptableObjects;

    public static class ItemCollectionExtensions
    {
        public static bool[] IngredientsAvailability<TStack, TItem>(this ItemCollection<TStack, TItem> inventory, List<ItemStack> ingredients)
            where TItem: ItemSO
            where TStack: BaseStack<TItem>, new()
        {
            bool[] availabilityArray = new bool[ingredients.Count];
			
            for (int i = 0; i < ingredients.Count; i++)
            {
                foreach (
                    var found in inventory._oneDimensionalStack.Select(
                        (stackLocation) => stackLocation.Stack.Item == ingredients[i].Item && stackLocation.Stack.Amount >= ingredients[i].Amount)
                )
                {
                    availabilityArray[i] = found;
                }
            }

            return availabilityArray;
        }

        public static bool HasIngredients<TStack, TItem>(this ItemCollection<TStack, TItem> inventory, List<ItemStack> ingredients)
            where TItem: ItemSO
            where TStack: BaseStack<TItem>, new()
        {
            bool hasIngredients = !ingredients.Exists((j) =>
            {
                foreach (var stack in inventory.Items)
                {
                    if(stack.Item == j.Item && stack.Amount >= j.Amount)
                    {
                        return true;
                    }
                }

                return false;
            });
			
            return hasIngredients;
        }
    }
}