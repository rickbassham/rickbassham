using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TheTvDb.Net.Test
{
	internal class TestHelper
	{
		public static object RunPrivateMethod(string methodName, object objInstance, ref object[] aobjParams)
		{
			MethodInfo m = null;
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			System.Type t = objInstance.GetType();

			try
			{
				m = t.GetMethod(methodName, bindingFlags);
			}
			catch (AmbiguousMatchException)
			{
				m = GetMethodUsingSignature(t, methodName, aobjParams, bindingFlags);
			}

			if (m == null)
			{
				throw new ArgumentException("There is no method '" + methodName + "' for type '" + t.ToString() + "'.");
			}

			object objRet = m.Invoke(objInstance, aobjParams);

			return objRet;
		}

		private static MethodInfo GetMethodUsingSignature(System.Type type, string methodName, object[] aobjParams, BindingFlags bindingFlags)
		{
			MethodInfo m = null;
			MethodInfo[] myArrayMethodInfo = type.GetMethods(bindingFlags);

			for (int i = 0; i < myArrayMethodInfo.Length; i++)
			{
				MethodInfo myMethodInfo = (MethodInfo)myArrayMethodInfo[i];
				if (myMethodInfo.Name == methodName)
				{
					if (myMethodInfo.GetParameters().Length == aobjParams.Length)
					{
						int counter = 0;
						ParameterInfo[] methodSignarure = myMethodInfo.GetParameters();
						foreach (object parameter in aobjParams)
						{
							if (parameter.GetType() != methodSignarure[counter].ParameterType & (methodSignarure[counter].ParameterType.IsByRef & parameter.GetType() != methodSignarure[counter].ParameterType.GetElementType()))
							{
								break;
							}
							counter++;
						}
						m = myMethodInfo;
						break;
					}

				}
			}
			return m;
		}
	}
}
