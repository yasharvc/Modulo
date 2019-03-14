using ModuloContracts.Hub.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ModuloContracts.Hub
{
	public static class InvocationHub
	{
		private const string CallFunctionPath = "/Debug/CallFunction";
		private const string GetConnectionStringPath = "/Debug/GetConnectionString";
		private const string GetModulesPath = "/Debug/GetModules";
		private const string GetUsersPath = "/Debug/GetUsers";
		private static string BaseUri = "http://192.168.0.56/";
		public static IInvocationHubProvider InvokationHubProvider { get; set; } = null;
		private static bool IsInModuleDebugMode => InvokationHubProvider == null;

		public static string GetConnectionString()
		{
			if (IsInModuleDebugMode)
			{
				return HttpPost(BaseUri, GetConnectionStringPath, new List<KeyValuePair<string, string>>());
			}
			else
				return SystemServiceProvider.SystemServicesProvider.GetConnectionString(false);
		}

		private static string HttpPost(string BaseUrl, string FunctionPath, List<KeyValuePair<string, string>> parameters = null)
		{
			if (parameters == null)
				parameters = new List<KeyValuePair<string, string>>();
			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var res = client.PostAsync(FunctionPath, new FormUrlEncodedContent(parameters)).Result;
			if (res.IsSuccessStatusCode)
			{
				string resultContent = res.Content.ReadAsStringAsync().Result;
				return (resultContent);
			}
			return "";
		}
		private static string HttpGet(string BaseUrl, string FunctionPath)
		{
			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var res = client.GetAsync(FunctionPath).Result;
			if (res.IsSuccessStatusCode)
			{
				string resultContent = res.Content.ReadAsStringAsync().Result;
				return (resultContent);
			}
			return "";
		}
	}
}
