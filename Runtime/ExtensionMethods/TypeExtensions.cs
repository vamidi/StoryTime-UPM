using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace StoryTime.Extensions
{
	public static class TypeExtensions
	{
		private static readonly object CachedNiceNames_LOCK = new object();

		private static readonly Dictionary<Type, string> CachedNiceNames = new Dictionary<Type, string>();

		private static readonly Type GenericListInterface = typeof (IList<>);

		private static readonly Type GenericCollectionInterface = typeof (ICollection<>);

		/// <summary>
		/// Type name alias lookup.
		/// TypeNameAlternatives["Single"] will give you "float", "UInt16" will give you "ushort", "Boolean[]" will give you "bool[]" etc..
		/// </summary>
		private static readonly Dictionary<string, string> TypeNameAlternatives = new Dictionary<string, string>()
		{
			{
				"Single",
				"float"
			},
			{
				"Double",
				"double"
			},
			{
				"SByte",
				"sbyte"
			},
			{
				"Int16",
				"short"
			},
			{
				"Int32",
				"int"
			},
			{
				"Int64",
				"long"
			},
			{
				"Byte",
				"byte"
			},
			{
				"UInt16",
				"ushort"
			},
			{
				"UInt32",
				"uint"
			},
			{
				"UInt64",
				"ulong"
			},
			{
				"Decimal",
				"decimal"
			},
			{
				"String",
				"string"
			},
			{
				"Char",
				"char"
			},
			{
				"Boolean",
				"bool"
			},
			{
				"Single[]",
				"float[]"
			},
			{
				"Double[]",
				"double[]"
			},
			{
				"SByte[]",
				"sbyte[]"
			},
			{
				"Int16[]",
				"short[]"
			},
			{
				"Int32[]",
				"int[]"
			},
			{
				"Int64[]",
				"long[]"
			},
			{
				"Byte[]",
				"byte[]"
			},
			{
				"UInt16[]",
				"ushort[]"
			},
			{
				"UInt32[]",
				"uint[]"
			},
			{
				"UInt64[]",
				"ulong[]"
			},
			{
				"Decimal[]",
				"decimal[]"
			},
			{
				"String[]",
				"string[]"
			},
			{
				"Char[]",
				"char[]"
			},
			{
				"Boolean[]",
				"bool[]"
			}
		};

		/// <summary>
		/// Determines whether a type implements or inherits from another type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="to">To.</param>
		public static bool ImplementsOrInherits(this Type type, Type to) => to.IsAssignableFrom(type);

		/// <summary>Returns a nicely formatted name of a type.</summary>
		public static string GetNiceName(this Type type) => type.IsNested && !type.IsGenericParameter
			? GetNiceName(type.DeclaringType) + "." + GetCachedNiceName(type)
			: GetCachedNiceName(type);

		/// <summary>
		/// Determines whether a type inherits or implements another type. Also include support for open generic base types such as List&lt;&gt;.
		/// </summary>
		/// <param name="type"></param>
		public static bool InheritsFrom<TBase>(this Type type) => type.InheritsFrom(typeof (TBase));

		/// <summary>
		/// Determines whether a type inherits or implements another type. Also include support for open generic base types such as List&lt;&gt;.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="baseType"></param>
		public static bool InheritsFrom(this Type type, Type baseType)
		{
			if (baseType.IsAssignableFrom(type))
				return true;
			if (type.IsInterface && !baseType.IsInterface)
				return false;
			if (baseType.IsInterface)
				return type.GetInterfaces().Contains(baseType);
			for (Type type1 = type; type1 != null; type1 = type1.BaseType)
			{
				if (type1 == baseType || baseType.IsGenericTypeDefinition && type1.IsGenericType && type1.GetGenericTypeDefinition() == baseType)
					return true;
			}
			return false;
		}

		/// <summary>
		/// FieldInfo will return the fieldType, propertyInfo the PropertyType, MethodInfo the return type and EventInfo will return the EventHandlerType.
		/// </summary>
		/// <param name="memberInfo">The MemberInfo.</param>
		public static Type GetReturnType(this MemberInfo memberInfo)
		{
			switch (memberInfo)
			{
				case FieldInfo fieldInfo:
					return fieldInfo.FieldType;
				case PropertyInfo propertyInfo:
					return propertyInfo.PropertyType;
				case MethodInfo methodInfo:
					return methodInfo.ReturnType;
				case EventInfo eventInfo:
					return eventInfo.EventHandlerType;
				default:
					return null;
			}
		}

		/// <summary>
		/// Determines whether a type implements an open generic interface such as IList&lt;&gt;.
		/// </summary>
		/// <param name="candidateType">Type of the candidate.</param>
		/// <param name="openGenericInterfaceType">Type of the open generic interface.</param>
		/// <exception cref="T:System.ArgumentNullException"></exception>
		/// <exception cref="T:System.ArgumentException">Type " + openGenericInterfaceType.Name + " is not a generic type definition and an interface.</exception>
		public static bool ImplementsOpenGenericInterface(this Type candidateType, Type openGenericInterfaceType)
		{
			if (candidateType == openGenericInterfaceType || candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericInterfaceType)
				return true;
			foreach (Type candidateType1 in candidateType.GetInterfaces())
			{
				if (candidateType1.ImplementsOpenGenericInterface(openGenericInterfaceType))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the generic arguments of an inherited open generic interface.
		/// </summary>
		/// <param name="candidateType">Type of the candidate.</param>
		/// <param name="openGenericInterfaceType">Type of the open generic interface.</param>
		public static Type[] GetArgumentsOfInheritedOpenGenericInterface(this Type candidateType, Type openGenericInterfaceType)
		{
			if ((openGenericInterfaceType == TypeExtensions.GenericListInterface || openGenericInterfaceType == TypeExtensions.GenericCollectionInterface) && candidateType.IsArray)
				return new []{ candidateType.GetElementType() };
			if (candidateType == openGenericInterfaceType || candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericInterfaceType)
				return candidateType.GetGenericArguments();
			foreach (Type candidateType1 in candidateType.GetInterfaces())
			{
				if (candidateType1.IsGenericType)
				{
					Type[] genericInterface = candidateType1.GetArgumentsOfInheritedOpenGenericInterface(openGenericInterfaceType);
					if (genericInterface != null)
						return genericInterface;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets all members from a given type, including members from all base types.
		/// </summary>
		public static IEnumerable<MemberInfo> GetAllMembers(
			this System.Type type,
			string name,
			BindingFlags flags = BindingFlags.Default)
		{
			foreach (MemberInfo allMember in type.GetAllMembers(flags))
			{
				if (!(allMember.Name != name))
					yield return allMember;
			}
		}

		/// <summary>
		/// Gets all members from a given type, including members from all base types if the <see cref="F:System.Reflection.BindingFlags.DeclaredOnly" /> flag isn't set.
		/// </summary>
		public static IEnumerable<MemberInfo> GetAllMembers(
			this Type type,
			BindingFlags flags = BindingFlags.Default)
		{
			Type currentType = type;
			MemberInfo[] memberInfoArray;
			int index;
			if ((flags & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
			{
				memberInfoArray = currentType.GetMembers(flags);
				for (index = 0; index < memberInfoArray.Length; ++index)
					yield return memberInfoArray[index];
				memberInfoArray = (MemberInfo[]) null;
			}
			else
			{
				flags |= BindingFlags.DeclaredOnly;
				do
				{
					memberInfoArray = currentType.GetMembers(flags);
					for (index = 0; index < memberInfoArray.Length; ++index)
						yield return memberInfoArray[index];
					memberInfoArray = (MemberInfo[]) null;
					currentType = currentType.BaseType;
				}
				while (currentType != null);
			}
		}

		/// <summary>Returns a nicely formatted full name of a type.</summary>
		public static string GetNiceFullName(this Type type)
		{
			if (type.IsNested && !type.IsGenericParameter)
				return type.DeclaringType.GetNiceFullName() + "." + GetCachedNiceName(type);
			string str = GetCachedNiceName(type);
			if (type.Namespace != null)
				str = type.Namespace + "." + str;
			return str;
		}

		private static string GetCachedNiceName(Type type)
		{
			string niceName;
			lock (CachedNiceNames_LOCK)
			{
				if (!CachedNiceNames.TryGetValue(type, out niceName))
				{
					niceName = CreateNiceName(type);
					CachedNiceNames.Add(type, niceName);
				}
			}

			return niceName;
		}

		private static string CreateNiceName(Type type)
		{
			if (type.IsArray)
			{
				int arrayRank = type.GetArrayRank();
				return GetNiceName(type.GetElementType()) + (arrayRank == 1 ? "[]" : "[,]");
			}
			if (type.InheritsFrom(typeof (Nullable<>)))
				return GetNiceName(type.GetGenericArguments()[0]) + "?";
			if (type.IsByRef)
				return "ref " + TypeExtensions.GetNiceName(type.GetElementType());
			if (type.IsGenericParameter || !type.IsGenericType)
				return type.TypeNameGauntlet();
			StringBuilder stringBuilder = new StringBuilder();
			string name = type.Name;
			int length = name.IndexOf('`');
			if (length != -1)
				stringBuilder.Append(name.Substring(0, length));
			else
				stringBuilder.Append(name);
			stringBuilder.Append('<');
			Type[] genericArguments = type.GetGenericArguments();
			for (int index = 0; index < genericArguments.Length; ++index)
			{
				Type type1 = genericArguments[index];
				if (index != 0)
					stringBuilder.Append(", ");
				stringBuilder.Append(GetNiceName(type1));
			}
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Used to filter out unwanted type names. Ex "int" instead of "Int32"
		/// </summary>
		private static string TypeNameGauntlet(this Type type)
		{
			string key = type.Name;
			string empty = string.Empty;
			if (TypeNameAlternatives.TryGetValue(key, out empty))
				key = empty;
			return key;
		}
	}
}
