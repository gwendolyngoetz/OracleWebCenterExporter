using System.Collections.Generic;
using System.Xml.Linq;

namespace OracleWebCenterExporter.Model
{
    internal class PageMetadata
    {
        public SiteHierachyNode SiteHierarchyTree { get; set; }
        public List<XDocument> PageMappings { get; set; } = new List<XDocument>();
        public List<WebCenterAsset> Assets { get; internal set; } = new List<WebCenterAsset>();
    }
}