using System.Collections.Generic;

namespace OracleWebCenterExporter.Model
{
    public class WebCenterPayload
    {
        public SiteHierachyNode SiteHierarchyTree { get; set; } = new SiteHierachyNode();
        public List<WebCenterPage> Pages { get; set; } = new List<WebCenterPage>();
        public List<WebCenterAsset> Assets { get; set; } = new List<WebCenterAsset>();
    }
}