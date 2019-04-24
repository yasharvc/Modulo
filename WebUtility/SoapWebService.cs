﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace WebUtility
{
	public class SoapWebService
	{
		public HttpWebRequest CreateWebRequest(string url, string action)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Headers.Add("SOAPAction", action);
			webRequest.ContentType = "text/xml;charset=\"utf-8\"";
			webRequest.Accept = "text/xml";
			webRequest.Method = "POST";
			return webRequest;
		}

		public XmlDocument CreateSoapEnvelope()
		{
			XmlDocument soapEnvelopeDocument = new XmlDocument();
			soapEnvelopeDocument.LoadXml("<Envelope xmlns=\"http://schemas.xmlsoap.org/soap/envelope/\"><Body><GetGoodsCodingList xmlns=\"http://tempuri.org/\"/></Body></Envelope>");
			return soapEnvelopeDocument;
		}

		public void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
		{
			using (Stream stream = webRequest.GetRequestStream())
			{
				soapEnvelopeXml.Save(stream);
			}
		}
	}

	public class WebService
	{
		public string Url { get; set; }
		public string MethodName { get; set; }
		public Dictionary<string, string> Params = new Dictionary<string, string>();
		public XDocument ResultXML;
		public string ResultString;

		public WebService()
		{

		}

		public WebService(string url, string methodName)
		{
			Url = url;
			MethodName = methodName;
		}

		/// <summary>
		/// Invokes service
		/// </summary>
		public void Invoke()
		{
			Invoke(true);
		}

		/// <summary>
		/// Invokes service
		/// </summary>
		/// <param name="encode">Added parameters will encode? (default: true)</param>
		public void Invoke(bool encode)
		{
			string soapStr =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <{0} xmlns=""http://tempuri.org/"">
                  {1}
                </{0}>
              </soap:Body>
            </soap:Envelope>";

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
			req.Headers.Add("SOAPAction", "\"http://tempuri.org/IWmsService/" + MethodName + "\"");
			req.ContentType = "text/xml;charset=\"utf-8\"";
			req.Accept = "application/json";
			req.Method = "POST";

			using (Stream stm = req.GetRequestStream())
			{
				string postValues = "";
				foreach (var param in Params)
				{
					if (encode)
						postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.UrlEncode(param.Key), HttpUtility.UrlEncode(param.Value));
					else
						postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
				}

				soapStr = string.Format(soapStr, MethodName, postValues);
				using (StreamWriter stmw = new StreamWriter(stm))
				{
					stmw.Write(soapStr);
				}
			}

			using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
			{
				string result = responseReader.ReadToEnd();
				ResultXML = XDocument.Parse(result);
				ResultString = result;
			}
		}
	}
}
