using System.IO;
using System.Text;
using System.Xml;

namespace VFPlaner
{
    public class Core
    {      
        public static XmlDocument SelectNodeMitarbeiter(string pathToDoc)

        {
            var doc = new XmlDocument();
            var output = new XmlDocument();
            doc.Load(pathToDoc);
            XmlNode root = doc.DocumentElement;
            XmlNodeList nlist;
            var test = "<Output>";
            foreach (XmlNode mitarbeiter in nlist = root.SelectNodes("//Saison2018/Volksfest"))// /ServiceTeam
            {

                test += mitarbeiter.InnerXml;

            }
            test = test + "</Output>";
            output.LoadXml(test);
            return output;

        }

        public static string ToIndentedString(XmlDocument doc)
        {
            if (doc == null) return "";
            using (var utf8Writer = new Utf8StringWriter())
            {
                using (var xmlTextWriter = new XmlTextWriter(utf8Writer))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    doc.Save(xmlTextWriter);
                    xmlTextWriter.Close();
                    return utf8Writer.ToString();
                }
            }
        }

        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }

        
    }
}