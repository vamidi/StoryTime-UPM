using System.Text;

namespace StoryTime.Utils.Extensions
{
	public static class StringExtensions
	{
		/// <summary>Ex: "thisIsCamelCase" -&gt; "This Is Camel Case"</summary>
		public static string SplitPascalCase(this string input)
		{
			switch (input)
			{
				case "":
				case null:
					return input;
				default:
					StringBuilder stringBuilder = new StringBuilder(input.Length);
					if (char.IsLetter(input[0]))
						stringBuilder.Append(char.ToUpper(input[0]));
					else
						stringBuilder.Append(input[0]);
					for (int index = 1; index < input.Length; ++index)
					{
						char c = input[index];
						if (char.IsUpper(c) && !char.IsUpper(input[index - 1]))
							stringBuilder.Append(' ');
						stringBuilder.Append(c);
					}
					return stringBuilder.ToString();
			}
		}

		/// <summary>
		/// Returns true if this string is null, empty, or contains only whitespace.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns><c>true</c> if this string is null, empty, or contains only whitespace; otherwise, <c>false</c>.</returns>
		public static bool IsNullOrWhitespace(this string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				for (int index = 0; index < str.Length; ++index)
				{
					if (!char.IsWhiteSpace(str[index]))
						return false;
				}
			}
			return true;
		}

		public static string UcFirst(this string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return string.Empty;
			}

			return char.ToUpper(input[0]) + input.Substring(1);
		}

		public static string LcFirst(this string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return string.Empty;
			}

			return char.ToLower(input[0]) + input.Substring(1);
		}
	}
}
