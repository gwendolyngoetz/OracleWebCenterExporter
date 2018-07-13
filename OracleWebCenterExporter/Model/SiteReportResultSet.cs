using System.Collections.Generic;

namespace OracleWebCenterExporter.Model
{
    internal class SiteReportResultSet
    {
        public List<WebsiteDocs> WebsiteDocs { get; set; } = new List<WebsiteDocs>();
        public List<SiteHierarchyItem> SiteHierarchy { get; set; } = new List<SiteHierarchyItem>();
        public List<UrlDataFiles> UrlDataFiles { get; set; } = new List<UrlDataFiles>();
        public List<NodeInfo> NodeInfo { get; set; } = new List<NodeInfo>();
    }

    internal class WebsiteDocs
    {
        public string DataFileId { get; set; }
        public string Title { get; set; }
        public int PageId { get; set; }
        public string XWebsiteObjectType { get; set; }
    }

    internal class SiteHierarchyItem
    {
        public int NodeId { get; set; }
        public int Level { get; set; }
        public string NamePath { get; set; }
    }

    internal class UrlDataFiles
    {
        public int NodeId { get; set; }
        public string DataFileId { get; set; }
        public string IsPrimaryUrl { get; set; }
    }

    internal class NodeInfo
    {
        public int NodeId { get; set; }
        public string PrimaryUrl { get; set; }
        public string SecondaryUrl { get; set; }
        public string Label { get; set; }
        public string PrimaryPlaceholderDefinitionUrl { get; set; }
        public string SecondaryPlaceholderDefinitionUrl { get; set; }
        public string PrimaryTemplateUrl { get; set; }
        public string SecondaryTemplateUrl { get; set; }
    }
}