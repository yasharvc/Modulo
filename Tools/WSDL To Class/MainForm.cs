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
			WSDL_SingleFile wsdl = new WSDL_SingleFile(WSDLUrlTextBox.Text);
			textBox1.Text = wsdl.GenerateClass();
		}
	}
}