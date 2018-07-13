using System;
using System.Xml.Linq;
using OracleWebCenterExporter.Extensions;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.XmlDeserialization
{
    public class WebCenterXmlDeserializer 
    {
        private static readonly XNamespace _nsWcm = "http://www.stellent.com/wcm-data/ns/8.0.0";

        public WebCenterPage Deserialize(string xml)
        {
            var xdoc = XDocument.Parse(xml);
            return Deserialize(xdoc);
        }

        public WebCenterPage Deserialize(XDocument xdoc)
        {
            var rootEl = xdoc.Element(_nsWcm + "root");

            if (rootEl == null)
            {
                throw new ArgumentNullException(nameof(xdoc), "Could not find root element in xml.");
            }

            var page = new WebCenterPage();

            foreach (var element in rootEl.Elements(_nsWcm + "element"))
            {
                page.AddElement(new WebCenterElement
                {
                    Name = GetAttributeValue(element, "name"),
                    Value = element.Value,
                    DataFileId = GetAttributeValue(element, "dataFileId"),
                    PageId = ToInt(GetAttributeValue(element, "pageId")),
                    PlaceholderName = GetAttributeValue(element, "placeholderName"),
                    PageTemplateName = GetAttributeValue(element, "pageTemplateName"),
                    Label = GetAttributeValue(element, "label"),
                    NodeId = ToInt(GetAttributeValue(element, "nodeId")),
                    Url = GetAttributeValue(element, "url")
                });
            }

            return page;
        }

        private string GetAttributeValue(XElement source, string name)
        {
            var attribute = source.Attribute(name);

            if (attribute != null)
            {
                return attribute.Value;
            }

            throw new InvalidOperationException($"Could not find attribute: {name}");
        }

        public static int ToInt(string source)
        {
            if (source.IsNullOrWhiteSpace())
            {
                return 0;
            }

            return int.TryParse(source, out var result) ? result : 0;
        }
    }
}