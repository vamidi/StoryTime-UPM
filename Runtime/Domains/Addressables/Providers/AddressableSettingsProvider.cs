#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace StoryTime.Domains.Addressables.Providers
{
    public class AddressableSettingsProvider
    {
#if UNITY_EDITOR
        internal virtual AddressableAssetSettings GetAddressableAssetSettings(bool create)
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(create);
            if (settings != null)
                return settings;

            // By default Addressables wont return the settings if updating or compiling. This causes issues for us, especially if we are trying to get the Locales.
            // We will just ignore this state and try to get the settings regardless.
            if (EditorApplication.isUpdating || EditorApplication.isCompiling)
            {
                // Legacy support
                if (EditorBuildSettings.TryGetConfigObject(AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, out settings))
                {
                    return settings;
                }

                AddressableAssetSettingsDefaultObject so;
                if (EditorBuildSettings.TryGetConfigObject(AddressableAssetSettingsDefaultObject.kDefaultConfigObjectName, out so))
                {
                    // Extract the guid
                    var serializedObject = new SerializedObject(so);
                    var guid = serializedObject.FindProperty("m_AddressableAssetSettingsGuid")?.stringValue;
                    if (!string.IsNullOrEmpty(guid))
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        return AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(path);
                    }
                }
            }
            return null;
        }
#endif
    }
}