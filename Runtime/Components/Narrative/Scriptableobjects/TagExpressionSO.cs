using System;
using System.Text.RegularExpressions;

namespace StoryTime.Components.ScriptableObjects
{
	/// <summary>
	/// TagExpression is a class where you can create your own way to handle tags in the dialogue
	///
	/// </summary>
	public class TagExpression
	{
		public string Tag => m_Tag;

		public string ExpressionToCheck => m_ExpressionToCheck;

		/// <summary>
		/// Regular expression that will be used
		/// </summary>
		protected string m_Tag;

		protected string m_ExpressionToCheck;

		protected int m_CurrentIndex = -1;

		public TagExpression(string tag, string pattern)
		{
			m_Tag = tag;
			m_ExpressionToCheck = pattern;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="original"></param>
		/// <param name="startAt"></param>
		/// <returns></returns>
		public Match MatchRegex(string original, int startAt = 0)
		{
			// split the whole text into parts based off the <> tags
			// even numbers in the array are text, odd numbers are tags
			// <action=>
			// first grab the value from the regular expression
			m_CurrentIndex = startAt;
			return Reg(m_Tag).Match(original, startAt);
		}

		public string GetRegexValue(string original, Func<Match, string> callback)
		{
			if (m_ExpressionToCheck == String.Empty)
				return String.Empty;

			Match match = Reg(m_ExpressionToCheck).Match(original, m_CurrentIndex);
			return callback(match);
		}

		private Regex Reg(string pattern) => new Regex(pattern, RegexOptions.Singleline);
	}
}
