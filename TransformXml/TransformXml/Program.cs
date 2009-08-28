using System;
using System.Xml;
using System.Xml.Xsl;

namespace TransformXml
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 3)
			{
				Console.WriteLine("TransformXml Usage:");
				Console.WriteLine("  TransformXml.exe <source xml file> <xsl file> <output file>");
				return;
			}

			string sourceXmlFile = args[0];
			string xslFile = args[1];
			string outputFile = args[2];

			XslCompiledTransform t = new XslCompiledTransform();

			t.Load(xslFile);

			t.Transform(sourceXmlFile, outputFile);
		}
	}
}
