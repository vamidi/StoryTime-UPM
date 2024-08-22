using System.Linq;
using System.Collections.Generic;
using StoryTime.Domains.Game.Characters.ScriptableObjects;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if UNITY_EDITOR && ODIN_INSPECTOR
namespace StoryTime.Editor.Domains.UI.Characters
{
    //
    // This class is used by the RPGEditorWindow to render an overview of all characters using the TableList attribute.
    // All characters are Unity objects though, so they are rendered in the inspector as single Unity object field,
    // which is not exactly what we want in our table. We want to show the members of the unity object.
    //
    // So in order to render the members of the Unity object, we'll create a class that wraps the Unity object
    // and displays the relevant members through properties, which works with the TableList, attribute.
    //
    public class CharacterTable
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<CharacterWrapper> _allCharacters;

        public CharacterSO this[int index] => _allCharacters[index].Character;

        public CharacterTable(IEnumerable<CharacterSO> characters)
        {
            _allCharacters = characters.Select(x => new CharacterWrapper(x)).ToList();
        }

        private class CharacterWrapper
        {
            private CharacterSO character; // Character is a ScriptableObject and would render a unity object
                                         // field if drawn in the inspector, which is not what we want.
            public CharacterSO Character => character;

            public CharacterWrapper(CharacterSO character)
            {
               this.character = character;
            }

            // [TableColumnWidth(50, false)]
            // [ShowInInspector, PreviewField(45, ObjectFieldAlignment.Center)]
            // public Texture Icon { get => character.Icon; set { character.Icon = value; EditorUtility.SetDirty(character); } }

            // [TableColumnWidth(120)]
            // [ShowInInspector]
            // public string Name { get { return character.Name; } set { character.Name = value; EditorUtility.SetDirty(character); } }

            // [ShowInInspector, ProgressBar(0, 100)]
            // public float Shooting { get { return character.CharacterClass.Shooting; } set { character.CharacterClass.Skills.Shooting = value; EditorUtility.SetDirty(character); } }

            // [ShowInInspector, ProgressBar(0, 100)]
            // public float Melee { get { return character.CharacterClass.Skills.Melee; } set { character.CharacterClass.Skills.Melee = value; EditorUtility.SetDirty(character); } }

            // [ShowInInspector, ProgressBar(0, 100)]
            // public float Social { get { return character.CharacterClass.Skills.Social; } set { character.CharacterClass.Skills.Social = value; EditorUtility.SetDirty(character); } }

            // [ShowInInspector, ProgressBar(0, 100)]
            // public float Animals { get { return character.CharacterClass.Skills.Animals; } set { character.CharacterClass.Skills.Animals = value; EditorUtility.SetDirty(character); } }

            // [ShowInInspector, ProgressBar(0, 100)]
            // public float Medicine { get { return character.CharacterClass.Skills.Medicine; } set { character.CharacterClass.Skills.Medicine = value; EditorUtility.SetDirty(character); } }

            // [ShowInInspector, ProgressBar(0, 100)]
            // public float Crafting { get { return character.CharacterClass.Skills.Crafting; } set { character.CharacterClass.Skills.Crafting = value; EditorUtility.SetDirty(character); } }
        }
    }
}
#endif