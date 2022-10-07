namespace StoryTime.Utils
{
	public static class HelperClass
	{
		public static string Capitalize(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}

			s = s.ToLower();
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);

			return new string(a);
		}
	}
}
