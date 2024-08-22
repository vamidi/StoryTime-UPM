using UnityEngine;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
#endif

using StoryTime.Domains.ItemManagement.Inventory;

#if UNITY_EDITOR
namespace StoryTime.Editor.Domains.UI.Item_Management.Drawers
{
    // 
    // In Character.cs we have a two dimension array of ItemSlots which is our inventory.
    // And instead of using the TableMatrix attribute to customize it there, we in this case 
    // instead create a custom drawer that will work for all two-dimensional ItemSlot arrays,
    // so we don't have to make the same CustomDrawer via the TableMatrix attribute again and again.
    // 

    internal sealed class ItemStackCellDrawer<TArray> : TwoDimensionalArrayDrawer<TArray, ItemStack>
        where TArray : System.Collections.IList
    {
        protected override TableMatrixAttribute GetDefaultTableMatrixAttributeSettings()
        {
            return new TableMatrixAttribute()
            {
                SquareCells = true,
                HideColumnIndices = true,
                HideRowIndices = true,
                ResizableColumns = false
            };
        }

        protected override ItemStack DrawElement(Rect rect, ItemStack value)
        {
            var id = DragAndDropUtilities.GetDragAndDropId(rect);
            DragAndDropUtilities.DrawDropZone(rect, value.Item ? value.Item.Icon : null, null, id); // Draws the drop-zone using the items icon.

            if (value.Item != null)
            {
                // Item count
                var countRect = rect.Padding(2).AlignBottom(16);
                value.Amount = EditorGUI.IntField(countRect, Mathf.Max(1, value.Amount));
                GUI.Label(countRect, "/ " + value.Item.ItemStackSize, SirenixGUIStyles.RightAlignedGreyMiniLabel);
            }

            value = DragAndDropUtilities.DropZone(rect, value);                                     // Drop zone for ItemSlot structs.
            value.Item = DragAndDropUtilities.DropZone(rect, value.Item);                           // Drop zone for Item types.
            value = DragAndDropUtilities.DragZone(rect, value, true, true);       // Enables dragging of the ItemSlot

            return value;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            base.DrawPropertyLayout(label);

            // Draws a drop-zone where we can destroy items.
            var rect = GUILayoutUtility.GetRect(0, 40).Padding(2);
            var id = DragAndDropUtilities.GetDragAndDropId(rect);
            DragAndDropUtilities.DrawDropZone(rect, null as UnityEngine.Object, null, id);
            DragAndDropUtilities.DropZone(rect, new ItemStack(), false, id);
        }
    }

}
#endif
