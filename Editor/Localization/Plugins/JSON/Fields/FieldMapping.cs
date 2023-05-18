using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google.Columns;

using UnityEngine.Localization;

namespace StoryTime.Editor.Localization.Plugins.JSON.Fields
{
	public static class FieldMapping
	{
		/// <summary>
		/// Creates a KeyColumn at "A" and then a <see cref="LocaleColumn"/> for each <see cref="Locale"/> in the project, each mapped to a unique Column.
		/// </summary>
		/// <returns></returns>
		public static List<SheetColumn> CreateDefaultMapping(string columnText = "text")
		{
			var columns = new List<SheetColumn> { new JsonField {Column = columnText } };
			AddLocaleMappings(columns);
			return columns;
		}

		/// <summary>
		/// Creates a <see cref="LocaleColumn"/> for any project <see cref="Locale"/> that is missing from the columns.
		/// </summary>
		/// <param name="columns">The existing column that will also be appeneded with any missing <see cref="Locale"/>'s</param>
		public static void AddLocaleMappings(IList<SheetColumn> columns)
		{
			var projectLocales = LocalizationEditorSettings.GetLocales();

			foreach (var locale in projectLocales)
			{
				// The locale is already mapped so we can ignore it
				if (columns.Any(c => c is LocaleColumn lc && lc.LocaleIdentifier == locale.Identifier))
					continue;

				columns.Add(new LocaleColumn { LocaleIdentifier = locale.Identifier, Column = locale.Identifier.Code });
			}
		}

		/// <summary>
        /// Creates columns by attempting to match to expected column names(case insensitive).<br/>
        /// The following names are checked:<br/>
        /// "key" => <see cref="KeyColumn"/><br/>
        /// "key id" => <see cref="KeyIdColumn"/><br/>
        /// Project <see cref="Locale"/>'s name, <see cref="LocaleIdentifier.ToString"/> or <see cref="LocaleIdentifier.Code"/> => <see cref="LocaleColumn"/>
        /// </summary>
        /// <param name="columnNames">The column names to create mappings for.</param>
        /// <param name="unusedNames">Optional list that can be populated with the names that a match could not be found for.</param>
        /// <returns></returns>
        public static List<SheetColumn> CreateMappingsFromColumnNames(IList<string> columnNames, IList<string> unusedNames = null)
        {
            var columns = new List<SheetColumn>();

            // We map all potential name variations into the dictionary and then check each name against it.
            // We could cache this however we would have to keep it in sync with the Locale's so for simplicity's sake, we don't.
            var nameMap = new Dictionary<string, Func<SheetColumn>>(StringComparer.OrdinalIgnoreCase)
            {
	            ["Key"] = () => new JsonField()
            };

            var projectLocales = LocalizationEditorSettings.GetLocales();
            foreach (var locale in projectLocales)
            {
                Func<SheetColumn> createLocaleFunc = () => new LocaleColumn { LocaleIdentifier = locale.Identifier };
                Func<SheetColumn> createCommentFunc = () => new LocaleCommentColumn { LocaleIdentifier = locale.Identifier };
                nameMap[locale.name] = createLocaleFunc;
                nameMap[locale.Identifier.ToString()] = createLocaleFunc;
                nameMap[locale.Identifier.Code] = createLocaleFunc;

                // Comment
                nameMap[locale.name + " Comments"] = createCommentFunc;
                nameMap[locale.Identifier + " Comments"] = createCommentFunc;
                nameMap[locale.Identifier.Code + " Comments"] = createCommentFunc;
            }

            // Now map the columns
            for (int i = 0; i < columnNames.Count; ++i)
            {
                if (nameMap.TryGetValue(columnNames[i], out var createFunc))
                {
                    var column = createFunc();
                    column.ColumnIndex = i;
                    columns.Add(column);
                }
                else if (unusedNames != null)
                {
                    unusedNames.Add(columnNames[i]);
                }
            }

            return columns;
        }
	}
}
