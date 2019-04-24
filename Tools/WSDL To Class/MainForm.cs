using System;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Net;
using System.Web;

namespace WSDL_To_Class
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void WSDLDownloadButton_Click(object sender, EventArgs e)
		{
			//if (!WSDLUrlTextBox.Text.EndsWith("?wsdl", StringComparison.OrdinalIgnoreCase))
			//	WSDLUrlTextBox.Text += "?wsdl";
			//try
			//{
			//	string contents = DownloadURLAsString(WSDLUrlTextBox.Text);
			//	textBox1.Text = contents;
			//	SoapFormatter formatter = new SoapFormatter();
			//	MemoryStream stringReader = new MemoryStream(Encoding.UTF8.GetBytes(contents));
			//	var obj = formatter.Deserialize(stringReader);
			//	//WSDLCompiler compiler = new WSDLCompiler(contents);
			//	//compiler.Compile();
			//	//TypesListBox.Items.AddRange(compiler.TypeString.Urls.ToArray());
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show(this,ex.Message);
			//}
			Consume();
		}

		private object Consume()
		{
			var ws = new WebService("http://shonizit:180/WmsService.svc", "GetGoodsCodingList");
			ws.Invoke();
			SoapFormatter formatter = new SoapFormatter();

			//serialize the object to the .dat file
			object deserializedSample = formatter.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(ws.ResultString)));
			return deserializedSample;
		}

		public static string DownloadURLAsString(string url)
		{
			string contents;
			using (var wc = new System.Net.WebClient())
				contents = wc.DownloadString(url);
			return contents;
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
				//foreach (var param in Params)
				//{
				//	if (encode)
				//		postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.UrlEncode(param.Key), HttpUtility.UrlEncode(param.Value));
				//	else
				//		postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
				//}

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
	public class GoodsCoding
	{

		public string DiagramCodeField { get; set; }

		public string DiagramNameField { get; set; }

		public string GoodsFamilyCodeField { get; set; }

		public string GoodsFamilyNameField { get; set; }

		public string GoodsGroupCodeField { get; set; }

		public string GoodsGroupNameField { get; set; }

		public string GoodsSpecificationsCodeField { get; set; }

		public string GoodsSpecificationsNameField { get; set; }

		public string GoodsSubGroupCodeField { get; set; }

		public string GoodsSubGroupNameField { get; set; }

		public string ProductCodeField { get; set; }

		public string ProductNameField { get; set; }
	}

	}
