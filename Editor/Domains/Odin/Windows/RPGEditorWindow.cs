using System.Linq;
using UnityEngine;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
#endif

namespace StoryTime.Editor.Domains.Odin.Windows
{
    using UI.Characters;
    using StoryTime.Domains.Game.Characters.ScriptableObjects;
    using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
    
    // ReSharper disable once InconsistentNaming
    public class RPGEditorWindow: 
#if ODIN_INSPECTOR
        OdinMenuEditorWindow
#else
        EditorWindow    
#endif
    {
        [MenuItem("Tools/StoryTime/RPG Editor")]
        public static void OpenWindow()
        {
            GetWindow<RPGEditorWindow>().Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true)
            {
                DefaultMenuStyle =
                {
                    IconSize = 28.00f
                },
                Config =
                {
                    DrawSearchToolbar = true
                }
            };

            // Adds the character overview table.
            CharacterOverview.Instance.UpdateCharacterOverview();
            tree.Add("Characters", new CharacterTable(CharacterOverview.Instance.allCharacters));

            // Adds all characters.
            tree.AddAllAssetsAtPath("Characters", "Assets/ScriptableObjects/Characters", typeof(CharacterSO), true, true);

            // Add all scriptable object items.
            tree.AddAllAssetsAtPath("Items", "Assets/ScriptableObjects/Item Management", typeof(ItemSO), true)
                .ForEach(AddDragHandles);
            
            // Add all scriptable object items.
            tree.AddAllAssetsAtPath("Inventory", "Assets/ScriptableObjects/Item Management", typeof(InventorySO), true)
                .ForEach(AddDragHandles);

            // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            tree.EnumerateTree().Where(x => x.Value as ItemSO).ForEach(AddDragHandles);

            // Add icons to characters and items.
            tree.EnumerateTree().AddIcons<CharacterSO>(x => x.Icon);
            tree.EnumerateTree().AddIcons<ItemSO>(x => x.Icon);

            return tree;
        }
        
        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    // ScriptableObjectCreator.ShowDialog<Item>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Items", obj =>
                    // {
                    //     obj.Name = obj.name;
                    //     base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    // });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                    // ScriptableObjectCreator.ShowDialog<Character>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Character", obj =>
                    // {
                    //     obj.Name = obj.name;
                    //     base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    // });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}