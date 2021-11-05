using System.Linq;
using System.Collections.Generic;

namespace StoryTime.Extensions
{
	public static class CollectionExtension
	{
		/// <summary>
		/// Is array null or empty
		/// </summary>
		public static bool IsNullOrEmpty<T>(this T[] collection)
		{
			return collection == null || collection.Length == 0;
		}

		/// <summary>
		/// Is list null or empty
		/// </summary>
		public static bool IsNullOrEmpty<T>(this IList<T> collection)
		{
			return collection == null || collection.Count == 0;
		}

		/// <summary>
		/// Is collection null or empty. IEnumerable is relatively slow. Use Array or List implementation if possible
		/// </summary>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return collection == null || !collection.Any();
		}
	}
}
