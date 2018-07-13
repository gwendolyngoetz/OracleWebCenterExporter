using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using OracleWebCenterExporter.Infrastructure;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.Services
{
    internal class SiteReportService
    {
        private readonly WebCenterConfiguration _config;
        private readonly ICookieAwareWebClient _webClient;

        internal SiteReportService(WebCenterConfiguration config) : this(config, new CookieAwareWebClient()) { /* no-op */ }

        internal SiteReportService(WebCenterConfiguration config, ICookieAwareWebClient webClient)
        {
            _config = config;
            _webClient = webClient;
        }

        public SiteReportResultSet GetSiteReport()
        {
            dynamic oracleSiteReportMetadata = GetSiteReportMetadata();

            var websiteDocs = GetWebsiteDocs(oracleSiteReportMetadata);
            var siteHierarchy = GetSiteHierarchy(oracleSiteReportMetadata);
            var urlDataFiles = GetUrlDataFiles(oracleSiteReportMetadata);
            var nodeInfo = GetNodeInfo(oracleSiteReportMetadata);

            return new SiteReportResultSet
            {
                WebsiteDocs = websiteDocs,
                SiteHierarchy = siteHierarchy,
                UrlDataFiles = urlDataFiles,
                NodeInfo = nodeInfo
            };
        }

        private dynamic GetSiteReportMetadata()
        {
            var client = GetClient();
            var responseJson = client.DownloadString(_config.SiteReportEndpoint);

            dynamic oracleSiteReportMetadata = JsonConvert.DeserializeObject(responseJson);

            return oracleSiteReportMetadata;
        }

        private ICookieAwareWebClient GetClient()
        {
            _webClient.Login(_config.LoginEndpoint, new NameValueCollection
            {
                { "j_character_encoding", "UTF-8" },
                { "j_username", _config.LoginUserName },
                { "j_password", _config.LoginPassword }
            });

            return _webClient;
        }

        private static List<WebsiteDocs> GetWebsiteDocs(dynamic oracleSiteReportMetadata)
        {
            var websiteDocs = new List<WebsiteDocs>();

            foreach (var row in oracleSiteReportMetadata.ResultSets.WebsiteDocs.rows)
            {
                websiteDocs.Add(new WebsiteDocs
                {
                    DataFileId = row[0],
                    Title = row[1],
                    PageId = row[2],
                    XWebsiteObjectType = row[3],
                });
            }
            return websiteDocs;
        }

        private static List<SiteHierarchyItem> GetSiteHierarchy(dynamic oracleSiteReportMetadata)
        {
            var siteHierarchy = new List<SiteHierarchyItem>();

            foreach (var row in oracleSiteReportMetadata.ResultSets.SiteHierarchy.rows)
            {
                siteHierarchy.Add(new SiteHierarchyItem
                {
                    NodeId = (int) row[0],
                    Level = (int) row[1],
                    NamePath = row[2]
                });
            }
            return siteHierarchy;
        }
   
        private static List<UrlDataFiles> GetUrlDataFiles(dynamic oracleSiteReportMetadata)
        {
            var urlDataFiles = new List<UrlDataFiles>();

            foreach (var row in oracleSiteReportMetadata.ResultSets.UrlDataFiles.rows)
            {
                urlDataFiles.Add(new UrlDataFiles
                {
                    NodeId = (int) row[0],
                    DataFileId = row[1],
                    IsPrimaryUrl = row[2]
                });
            }

            return urlDataFiles;
        }

        private static List<NodeInfo> GetNodeInfo(dynamic oracleSiteReportMetadata)
        {
            var nodeInfo = new List<NodeInfo>();

            foreach (var row in oracleSiteReportMetadata.ResultSets.NodeInfo.rows)
            {
                nodeInfo.Add(new NodeInfo
                {
                    NodeId = (int) row[0],
                    PrimaryUrl = row[1],
                    SecondaryUrl = row[2],
                    Label = row[4],
                    PrimaryPlaceholderDefinitionUrl = row[5],
                    SecondaryPlaceholderDefinitionUrl = row[6],
                    PrimaryTemplateUrl = row[7],
                    SecondaryTemplateUrl = row[8]
                });
            }
            return nodeInfo;
        }
    }
}