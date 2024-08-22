using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.Utilities;
using Sirenix.OdinInspector;
#endif


#if UNITY_EDITOR
using UnityEditor;
#endif

using StoryTime.Domains.Game.Characters.ScriptableObjects;

#if UNITY_EDITOR && ODIN_INSPECTOR
namespace StoryTime.Editor.Domains.UI.Characters
{
    // 
    // This is a scriptable object containing a list of all characters available
    // in the Unity project. When a character is added from the RPG editor, the
    // list then gets automatically updated via UpdateCharacterOverview. 
    //
    // If you inspect the Character Overview in the inspector, you will also notice, that
    // the list is not directly modifiable. Instead, we've customized it so it contains a 
    // refresh button, that scans the project and automatically populates the list.
    //
    // CharacterOverview inherits from GlobalConfig which is just a scriptable 
    // object singleton, used by Odin Inspector for configuration files etc, 
    // but it could easily just be a simple scriptable object instead.
    // 

    [GlobalConfig("Tool/StoryTime/RPG Editor/Characters")]
    public class CharacterOverview : GlobalConfig<CharacterOverview> 
    {
        [ReadOnly]
        [ListDrawerSettings(ShowFoldout = true)]
        public CharacterSO[] allCharacters;

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateCharacterOverview()
        {
            // Finds and assigns all scriptable objects of type Character
            this.allCharacters = AssetDatabase.FindAssets("t:Character")
                .Select(guid => AssetDatabase.LoadAssetAtPath<CharacterSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif
    }
}
#endif
