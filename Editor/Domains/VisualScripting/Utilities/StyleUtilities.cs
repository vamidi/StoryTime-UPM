using UnityEngine;
using UnityEngine.UIElements;

namespace StoryTime.Editor.Domains.VisualScripting.Utilities
{
    public static class StyleUtilities
    {
        public static VisualElement AddClasses(this VisualElement visualElement, params string[] classNames)
        {
            foreach (var className in classNames) visualElement.AddToClassList(className);

            return visualElement;
        }

        public static VisualElement AddStyleSheets(this VisualElement visualElement, params string[] styleSheetNames)
        {
            foreach (var styleSheetName in styleSheetNames)
            {
                var styleSheet = Resources.Load<StyleSheet>(styleSheetName);
                visualElement.styleSheets.Add(styleSheet);
            }

            return visualElement;
        }
    }
}
