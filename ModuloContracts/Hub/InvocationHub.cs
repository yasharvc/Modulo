using Microsoft.AspNetCore.Http;
using ModuloContracts.Hub.Interfaces;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ModuloContracts.Hub
{
	public static class InvocationHub
	{
		private const string CallFunctionPath = "/Debug/CallFunction";
		private const string GetConnectionStringPath = "/Debug/GetConnectionString";
		private const string GetModulesPath = "/Debug/GetModules";
		private const string GetUsersPath = "/Debug/GetUsers";
		private static string BaseUri = "http://192.168.0.56/";
		public static InvocationHubProvider InvokationHubProvider { get; set; } = null;
		public static bool IsInModuleDebugMode => InvokationHubProvider == null;

		public static string GetConnectionString()
		{
			if (IsInModuleDebugMode)
			{
				return HttpPost(BaseUri, GetConnectionStringPath, new List<KeyValuePair<string, string>>());
			}
			else
				return SystemServiceProvider.SystemServicesProvider.GetConnectionString(false);
		}

		public static IEnumerable<IManifest> GetModules()
		{
			if (IsInModuleDebugMode)
				return new List<IManifest> {
					new DummyManifest("Dummy1","First dummy module","Description for first dummy module"),
					new DummyManifest("Dummy2","Second dummy module","Description for second dummy module"),
					new DummyManifest("Dummy3","Third dummy module","Description for third dummy module")
				};
			else
				return InvokationHubProvider.GetModuleList().Select(m => m.Manifest);
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
