using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace VFPlaner
{
    public class Core
    {
        public static XmlDocument SelectNodes(string pathToDoc, string NodeAsXpath)

        {
            var doc = new XmlDocument();
            var output = new XmlDocument();
            doc.Load(pathToDoc);
            XmlNode root = doc.DocumentElement;
            XmlNodeList nlist;
            var test = "<Output>";
            foreach (XmlNode mitarbeiter in nlist = root.SelectNodes(NodeAsXpath))// /ServiceTeam
            {

                test += mitarbeiter.InnerXml;

            }
            test = test + "</Output>";
            output.LoadXml(test);
            return output;

        }

        public static bool ValidationOfXml(string PathToSchema, string fileName)
        {
            bool isValid = true;
            XDocument document = XDocument.Load(fileName);
            XmlSchemaSet schemaSet = new XmlSchemaSet();

            schemaSet.Add(null, PathToSchema);

            XmlReaderSettings xrs = new XmlReaderSettings() { ValidationType = ValidationType.Schema };
            xrs.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            xrs.Schemas = schemaSet;
            xrs.ValidationEventHandler += (o, s) =>
            {
                throw new Exception(s.Message.ToString() );
            };
            
            using(XmlReader xr = XmlReader.Create(document.CreateReader(), xrs))
            {
                while (xr.Read()) { }
            }
            

            return isValid;
        }


        public static string ToIndentedString(XmlDocument doc)
        {
            if (doc == null) return string.Empty;
            using (var utf8Writer = new Utf8StringWriter())
            using (var xmlTextWriter = new XmlTextWriter(utf8Writer))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                doc.Save(xmlTextWriter);
                xmlTextWriter.Close();
                return utf8Writer.ToString();
            }
        }

        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }


    }
}