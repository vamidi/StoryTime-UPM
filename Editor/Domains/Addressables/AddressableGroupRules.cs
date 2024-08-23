using System;

using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

using UnityEngine;
using UnityEngine.Localization.Tables;
using Object = UnityEngine.Object;

namespace StoryTime.Editor.Domains.Addressables
{
    /// <summary>
    /// Provides support for configuring rule sets to determine the <see cref="AddressableAssetGroup"/>s that localized assets should be added to.
    /// </summary>
    public class AddressableGroupRules : ScriptableObject
    {
        const string k_ConfigName = "com.unity.localization.addressable-group-rules";

        [SerializeReference] GroupResolver m_TablesResolver = new ("StoryTime-Tables-{ObjectName}", "StoryTime-Assets-Shared");

        static AddressableGroupRules s_Instance;

        /// <summary>
        /// The active <see cref="AddressableGroupRules"/> that is being used by the project.
        /// </summary>
        public static AddressableGroupRules Instance
        {
            get
            {
                if (s_Instance == null && !EditorBuildSettings.TryGetConfigObject(k_ConfigName, out s_Instance))
                    s_Instance = CreateInstance<AddressableGroupRules>();
                return s_Instance;
            }
            set
            {
                if (s_Instance == value)
                    return;

                if (EditorUtility.IsPersistent(value))
                {
                    EditorBuildSettings.AddConfigObject(k_ConfigName, value, true);
                    Debug.Log("Localization Addressables Group Rules changed to " + AssetDatabase.GetAssetPath(value));
                }

                s_Instance = value;
            }
        }

        static void CreateAsset(Action<AddressableGroupRules> configureAction)
        {
            var path = EditorUtility.SaveFilePanelInProject("Create Addressable Group Rules", "StoryTime Addressable Group Rules.asset", "asset", "");
            if (string.IsNullOrEmpty(path))
                return;

            var instance = CreateInstance<AddressableGroupRules>();

            configureAction?.Invoke(instance);
            AssetDatabase.CreateAsset(instance, path);
            Instance = instance;
        }

        [MenuItem("Assets/Create/Localization/Addressable Group Rules (Default)")]
        static void CreateDefault() => CreateAsset(null);

        [MenuItem("Assets/Create/Localization/Addressable Group Rules (Single Group)")]
        static void CreateSingleGroup()
        {
            CreateAsset(instance =>
            {
                instance.TablesResolver = new GroupResolver("StoryTime", "StoryTime");
            });
        }

        /// <summary>
        /// Controls which groups <see cref="AssetTable"/> and their <see cref="SharedTableData"/> are added to.
        /// </summary>
        public GroupResolver TablesResolver
        {
            get => m_TablesResolver;
            set
            {
                if (ReferenceEquals(m_TablesResolver, value))
                    return;

                m_TablesResolver = value;
                EditorUtility.SetDirty(this);
            }
        }

        internal static AddressableAssetEntry AddTableSharedAsset(Object asset, AddressableAssetSettings aaSettings, bool createUndo) => Instance.TablesResolver.AddToGroup(asset, aaSettings, createUndo);
    }
}
