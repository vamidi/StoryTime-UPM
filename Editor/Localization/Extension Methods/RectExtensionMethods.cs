using UnityEngine;
using UnityEditor;

namespace DatabaseSync.Localization
{
    static class RectExtensionMethods
    {
        public static void MoveToNextLine(this ref Rect rect)
        {
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
        }

        public static (Rect left, Rect right) SplitHorizontalFixedWidthRight(this Rect rect, float rightWidth, float padding = 2)
        {
            var leftWidth = rect.width - padding - rightWidth;
            var left = new Rect(rect.x, rect.y, leftWidth, rect.height);
            var right = new Rect(left.xMax + padding, rect.y, rightWidth, rect.height);
            return (left, right);
        }

        public static (Rect left, Rect right) SplitHorizontal(this Rect rect, float leftAmount = 0.5f, float padding = 2)
        {
            var width = rect.width - padding;
            float leftWidth = width * leftAmount;
            float rightWidth = width - leftWidth;
            var left = new Rect(rect.x, rect.y, leftWidth, rect.height);
            var right = new Rect(left.xMax + padding, rect.y, rightWidth, rect.height);
            return (left, right);
        }
    }
}
