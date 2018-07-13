using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NLog;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.Services
{
    internal class WebCenterMetadataService
    {
        protected readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly WebCenterConfiguration _config;

        public WebCenterMetadataService(WebCenterConfiguration config)
        {
            _config = config;
        }

        public PageMetadata GetPageMetadata()
        {
            var siteReport = new SiteReportService(_config).GetSiteReport();
            var siteHierarchyTree = new SiteHierarchyTreeBuilder().ToTree(siteReport.SiteHierarchy);
            var siteReportMappingGroups = new SiteReportMapper(_config).GroupMappings(siteReport);

            var pageMappings = new List<XDocument>();

            foreach (var siteReportMappingGroup in siteReportMappingGroups)
            {
                _logger.Debug($"Merging xml fragments into single XDocument: {siteReportMappingGroup.Key}");
                pageMappings.Add(GetRootXDoc(siteReportMappingGroup.Select(x => x).ToList()));
            }

            var assets = new WebCenterAssetService(_config).GetAssets(siteReport);

            return new PageMetadata
            {
                PageMappings = pageMappings,
                SiteHierarchyTree = siteHierarchyTree,
                Assets = assets
            };
        }

        private XDocument GetRootXDoc(List<PageMapping> files)
        {
            FixXmlFiles(files);
            var firstFile = files.FirstOrDefault();

            var rootXDoc = XDocument.Parse(firstFile.Xml);
            
            PopulateElementAttributes(firstFile, rootXDoc.Root.Elements());

            foreach (var file in files.Skip(1))
            {
                var childXDoc = XDocument.Parse(file.Xml);
                var elements = childXDoc.Root.Elements();
                PopulateElementAttributes(file, elements);
                rootXDoc.Root.Add(elements);
            }

            return rootXDoc;
        }

        private void FixXmlFiles(List<PageMapping> files)
        {
            foreach (var file in files)
            {
                if (file.Xml.StartsWith("?"))
                {
                    file.Xml = "<" + file.Xml;
                }
            }
        }

        private static void PopulateElementAttributes(PageMapping file, IEnumerable<XElement> rootElements)
        {
            foreach (var rootElement in rootElements)
            {
                rootElement.Add(new XAttribute("dataFileId", file.DataFileId));
                rootElement.Add(new XAttribute("pageId", file.PageId));
                rootElement.Add(new XAttribute("placeholderName", file.PlaceholderName));
                rootElement.Add(new XAttribute("pageTemplateName", file.PageTemplateName));
                rootElement.Add(new XAttribute("label", file.Label));
                rootElement.Add(new XAttribute("nodeId", file.NodeId));
                rootElement.Add(new XAttribute("url", file.NamePath));
            }
        }
    }
}