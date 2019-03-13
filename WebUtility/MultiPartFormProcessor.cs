using ModuloContracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace WebUtility
{
	public class MultiPartFormProcessor
	{
		private enum ContentType
		{
			None,
			PlainText,
			Octet
		}
		private class ContentDescription
		{
			public string Name { get; set; } = "";
			public string FileName { get; set; } = "";
			public bool IsFile { get; set; } = false;

			public ContentDescription(string Line)
			{
				var index = Line.IndexOf("name=\"") + "name=\"".Length;
				Name = Line.Substring(index, Line.IndexOf("\"", index + 1) - index);
				index = Line.IndexOf("filename=\"");
				if (index > 0)
				{
					index += "filename=\"".Length;
					FileName = Line.Substring(index, Line.IndexOf("\"", index + 1) - index);
					IsFile = true;
				}
			}
		}
		private RequestData RequestData { get; set; }
		private IEnumerable<byte> boundaryBytes { get; set; }

		public MultiPartFormProcessor(RequestData request)
		{
			RequestData = request;
		}

		public List<RequestParameter> Process()
		{
			List<RequestParameter> sections = new List<RequestParameter>();
			if (RequestData.ContentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
			{
				var boundary = $"---{RequestData.Boundary}";
				boundaryBytes = boundary.ToByte();
				IEnumerable<byte> x = new List<byte>();
				List<byte> bodyBytesClone = new List<byte>();
				bodyBytesClone.AddRange(RequestData.Body);
				while (bodyBytesClone.Count() > 0)
				{
					x = ReadNextSection(ref bodyBytesClone);
					var section = Compile(x);
					if (section.Type != RequestParameterType.None)
						sections.Add(section);
				}
			}
			return sections;
		}

		private IEnumerable<byte> ReadNextSection(ref List<byte> bodyBytes)
		{
			var res = new List<byte>();
			if (bodyBytes.SequenceEqual(new byte[] { 45, 45, 13, 10 }))
			{
				bodyBytes = new List<byte>();
				return bodyBytes;
			}
			byte boundaryStart = boundaryBytes.ElementAt(0);
			for (int i = 0; i < bodyBytes.Count(); i++)
			{
				if (bodyBytes.ElementAt(i) != boundaryStart)
					continue;
				var found = true;
				foreach (var boundaryByte in boundaryBytes)
				{
					if (bodyBytes.ElementAt(i++) != boundaryByte)
					{
						found = false;
						break;
					}
				}
				if (found)
				{
					res.AddRange(bodyBytes.Take(i - boundaryBytes.Count()));
					bodyBytes = bodyBytes.Skip(i).ToList();
					return res;
				}
			}
			return bodyBytes;
		}

		private RequestParameter Compile(IEnumerable<byte> bodyBytes)
		{
			var res = new RequestParameter();
			if (bodyBytes.Count() == 0)
				return res;
			while (bodyBytes.ElementAt(0) == 13 || bodyBytes.ElementAt(0) == 10)
				bodyBytes = bodyBytes.Skip(1);
			var lineBytes = new byte[0];
			var line = "";
			GetNextLine(ref bodyBytes, out lineBytes, out line);
			var x = new ContentDescription(line);
			res.Name = x.Name;
			if (x.IsFile)
			{
				res.Type = RequestParameterType.File;
				GetNextLine(ref bodyBytes, out lineBytes, out line);
				var contentType = GetContentType(line);
				if (contentType == ContentType.PlainText || contentType == ContentType.Octet)
				{
					res.File = bodyBytes.Skip(4).SkipLast(2).ToArray();
					res.FileName = "";
				}
			}
			else
			{
				res.Type = RequestParameterType.Simple;
				res.Value = bodyBytes.Skip(4).SkipLast(2).ToString(Encoding.UTF8);
			}
			return res;
		}
		private ContentType GetContentType(string line)
		{
			if (line.Split(":")[1].Trim().ToLower() == "text/plain")
				return ContentType.PlainText;
			return ContentType.Octet;
		}
		private void GetNextLine(ref IEnumerable<byte> x, out byte[] lineBytes, out string line)
		{
			if (x.ElementAt(0) == 10 || x.ElementAt(0) == 13)
				x = x.SkipWhile(b => b == 10 || b == 13);
			lineBytes = x.TakeWhile(b => b != 13).ToArray();
			line = Encoding.UTF8.GetString(lineBytes);
			x = x.Skip(lineBytes.Length);
		}
	}
}
