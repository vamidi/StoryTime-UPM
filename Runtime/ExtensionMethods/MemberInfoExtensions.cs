using System;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace DatabaseSync.Extensions
{
	public static class MemberInfoExtensions
	{
		/// <summary>
		/// Returns the specified method's full name "methodName(argType1 arg1, argType2 arg2, etc)"
		/// Uses the specified gauntlet to replaces type names, ex: "int" instead of "Int32"
		/// </summary>
		public static string GetFullName(this MethodBase method, string extensionMethodPrefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (method.IsExtensionMethod())
				stringBuilder.Append(extensionMethodPrefix);
			stringBuilder.Append(method.Name);
			if (method.IsGenericMethod)
			{
				Type[] genericArguments = method.GetGenericArguments();
				stringBuilder.Append("<");
				for (int index = 0; index < genericArguments.Length; ++index)
				{
					if (index != 0)
						stringBuilder.Append(", ");
					stringBuilder.Append(TypeExtensions.GetNiceName(genericArguments[index]));
				}
				stringBuilder.Append(">");
			}
			stringBuilder.Append("(");
			stringBuilder.Append(method.GetParamsNames());
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
