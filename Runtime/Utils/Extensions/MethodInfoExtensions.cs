using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StoryTime.Utils.Extensions
{
	public static class MethodInfoExtensions
	{
		/// <summary>
		/// Returns a string representing the passed method parameters names. Ex "int num, float damage, Transform target"
		/// </summary>
		public static string GetParamsNames(this MethodBase method)
		{
			ParameterInfo[] parameterInfoArray = method.IsExtensionMethod() ? (method.GetParameters()).Skip(1).ToArray() : method.GetParameters();
			StringBuilder stringBuilder = new StringBuilder();
			int index = 0;
			for (int length = parameterInfoArray.Length; index < length; ++index)
			{
				ParameterInfo parameterInfo = parameterInfoArray[index];
				string niceName = parameterInfo.ParameterType.GetNiceName();
				stringBuilder.Append(niceName);
				stringBuilder.Append(" ");
				stringBuilder.Append(parameterInfo.Name);
				if (index < length - 1)
					stringBuilder.Append(", ");
			}
			return stringBuilder.ToString();
		}

		/// <summary>Tests if a method is an extension method.</summary>
		public static bool IsExtensionMethod(this MethodBase method)
		{
			Type declaringType = method.DeclaringType;
			return declaringType is {IsSealed: true, IsGenericType: false, IsNested: false} && method.IsDefined(typeof (ExtensionAttribute), false);
		}
	}
}
