using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryTime.Utils
{
	public static class HelperClass
	{
		public static IEnumerable<TResult> GetEnumValues<TResult>()
		{
			return Enum.GetValues(typeof(TResult)).Cast<TResult>();
		}
	}
}
