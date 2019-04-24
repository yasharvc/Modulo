using ModuloContracts.Exceptions.SystemExceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DllLoader
{
	public class Invoker
	{
		private Loader Loader { get; set; }
		public Invoker(string PathToDll) : this(new Loader(PathToDll)) { }
		public Invoker(Loader loader)
		{
			Loader = loader;
		}

		public T CreateInstance<T>(string ClassFullName) where T : class => Loader.GetMainAssembly().CreateInstance(ClassFullName, true) as T;
		public T CreateInstance<T>(Type type) where T : class => CreateInstance<T>(type.FullName);
		public object CreateInstance(Type type) => Loader.GetMainAssembly().CreateInstance(type.FullName, true);

		public T CreateInstanceByParentType<T>() where T : class
		{
			var types = Loader.GetMainAssembly().GetTypes();
			foreach(var type in types)
			{
				if (type.BaseType == null) continue;
				if (typeof(ModuloContracts.Module.Manifesto).FullName == type.BaseType.FullName ||
					typeof(ModuloContracts.Module.Interfaces.IManifest).FullName == type.BaseType.FullName)
					return CreateInstance(type) as T;
			}
			throw new NotImplementedException();
		}

		public T InvokeMethod<T>(object obj, string MethodName, IEnumerable<Type> CustomAttributes, Dictionary<string,object> Parameters) where T : class
		{
			try
			{
				var method = GetMethod(obj, MethodName, CustomAttributes, Parameters.Count);
				var convertedParameters = ConvertParameteres(method, Parameters);
				return InvokeMethodWithConvertedParameters<T>(obj, method, convertedParameters);
			}
			catch (MethodNotFoundException e)
			{
				throw e;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public IEnumerable<Type> GetTypes() => Loader.GetTypes();

		private T InvokeMethodWithConvertedParameters<T>(object obj,MethodInfo method, Dictionary<string, object> convertedParameters) where T:class
		{
			var parameters = method.GetParameters();
			object[] orderedParams = new object[parameters.Length];
			int i = 0;
			foreach (var param in parameters)
				orderedParams[i++] = convertedParameters[param.Name];
			var resultObject = orderedParams.Length > 0 ? method.Invoke(obj, orderedParams) : method.Invoke(obj,new object[] { });
			if (!(resultObject is T res))
				res = Extenstions.FromObject<T>(resultObject);
			return res;
		}


		private Dictionary<string,object> ConvertParameteres(MethodInfo method, Dictionary<string,object> parameters)
		{
			var methodParameteres = method.GetParameters();
			Dictionary<string,object> res = new Dictionary<string, object>();
			int i = 0;
			foreach(var methodParameter in methodParameteres)
			{
				if (methodParameter.ParameterType.IsPrimitiveType())
					res[methodParameter.Name] = Convert.ChangeType(parameters[methodParameter.Name], methodParameter.ParameterType);
				else
				{
					var parStr = Convert.ChangeType(parameters[methodParameter.Name], typeof(string)) as string;
					if (parStr.StartsWith("{") || parStr.StartsWith("["))
						res[methodParameter.Name] = JsonConvert.DeserializeObject(parameters[methodParameter.Name].ToJson(), methodParameter.ParameterType);
					else
					{
						res[methodParameter.Name] = GetObjectFromParameters(parameters, methodParameter.ParameterType);
					}
					i++;
				}
			}
			return res;
		}

		private object GetObjectFromParameters(Dictionary<string, object> parameters, Type parameterType)
		{
			var props = parameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var propVals = new Dictionary<string, object>();
			foreach (var prop in props)
			{
				try
				{
					//parameterType.GetProperty(prop.Name).SetValue(res, Convert.ChangeType(parameters[prop.Name], prop.PropertyType));
					if (prop.PropertyType.IsPrimitiveType())
						propVals[prop.Name] = Convert.ChangeType(parameters[prop.Name], prop.PropertyType);
					else
						throw new NotImplementedException();
				}
				catch(Exception e) {
					var str = e.Message;
				}
			}
			var json = JsonConvert.SerializeObject(propVals);
			return JsonConvert.DeserializeObject(json, parameterType);
		}

		private MethodInfo GetMethod(object obj, string methodName, IEnumerable<Type> customAttributes, int parametersCount)
		{
			var methods = obj.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			foreach (var method in methods)
				if (IsMethodEligable(methodName, customAttributes, parametersCount, method))
					return method;
			throw new MethodNotFoundException(obj.GetType().FullName, methodName);
		}

		private bool IsMethodEligable(string MethodName, IEnumerable<Type> CustomAttributes, int ParametersCount, MethodInfo method)
		{
			return //method.GetParameters().Length == ParametersCount &&
								method.Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase)
								&& HasAllAttributes(CustomAttributes, method);
		}

		private bool HasAllAttributes(IEnumerable<Type> CustomAttributes, MethodInfo method)
		{
			if (CustomAttributes == null)
				return true;
			foreach (var attr in CustomAttributes)
				if (method.GetCustomAttribute(attr) == null)
					return false;
			return true;
		}
	}
}