using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using OracleWebCenterExporter.Extensions;
using OracleWebCenterExporter.GetFileSvc;
using OracleWebCenterExporter.Infrastructure;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.Services
{
    internal class SiteReportMapper
    {
        protected readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly WebCenterConfiguration _config;

        public SiteReportMapper(WebCenterConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<IGrouping<int, PageMapping>> GroupMappings(SiteReportResultSet siteReportResultSet)
        {
            return JoinLists(siteReportResultSet)
                .OrderBy(x => x.NamePath)
                .GroupBy(x => x.NodeId)
                .Select(x => x)
                .Select(x => x);
        }

        private List<PageMapping> JoinLists(SiteReportResultSet siteReportResultSet)
        {
            var oraclePageMappings =
            (from w1 in siteReportResultSet.WebsiteDocs.Where(x => x.XWebsiteObjectType == "Data File")
             join u1 in siteReportResultSet.UrlDataFiles on w1.DataFileId equals u1.DataFileId
             join s1 in siteReportResultSet.SiteHierarchy on u1.NodeId equals s1.NodeId
             join n1 in siteReportResultSet.NodeInfo on s1.NodeId equals n1.NodeId
             select new PageMapping
             {
                 DataFileId = w1.DataFileId,
                 PageId = w1.PageId,
                 NodeId = u1.NodeId,
                 NamePath = s1.NamePath,
                 TargetQueryString = n1.PrimaryUrl,
                 Label = n1.Label
             }).ToList();

            Parallel.ForEach(oraclePageMappings, oraclePageMapping =>
            {
                var (placeholderName, _) = ParseTargetQueryString(oraclePageMapping.TargetQueryString).FirstOrDefault(x => x.dataFileId == oraclePageMapping.DataFileId);
                oraclePageMapping.PlaceholderName = placeholderName ?? string.Empty;
                oraclePageMapping.PageTemplateName = ParseTargetQueryStringForPageTemplateName(oraclePageMapping.TargetQueryString);
                oraclePageMapping.Xml = GetFileContent(oraclePageMapping);
            });

            return oraclePageMappings;
        }

        private List<(string placeholderName, string dataFileId)> ParseTargetQueryString(string targetQueryString)
        {
            var result = new List<(string placefolderName, string datafileId)>();

            targetQueryString = TruncateCharacterAndPrecedingString(targetQueryString);

            if (targetQueryString.IsNullOrWhiteSpace())
            {
                return result;
            }

            var items = targetQueryString.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            if (items == null)
            {
                throw new ArgumentNullException($"Splitting the targetQueryString on '&' resulted in a null collection. Value: {targetQueryString}.");
            }

            foreach (var item in items)
            {
                var parts = item.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                var placeholderName = parts.Length > 0 ? parts[0] : "";
                var dataFileId = parts.Length > 1 ? parts[1] : "";

                if (placeholderName.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException($"Missing placeholderName value in item {item}.");
                }

                if (dataFileId.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException($"Missing dataFileId value in item {item}.");
                }

                result.Add((placeholderName, dataFileId));
            }

            return result;
        }

        private static string TruncateCharacterAndPrecedingString(string targetQueryString)
        {
            var questionMarkIndex = targetQueryString.IndexOf("?", StringComparison.Ordinal);
            return questionMarkIndex == -1 ? targetQueryString : targetQueryString.Substring(questionMarkIndex + 1, targetQueryString.Length - questionMarkIndex - 1);
        }

        private static string ParseTargetQueryStringForPageTemplateName(string targetQueryString)
        {
            var questionMarkIndex = targetQueryString.IndexOf("?", StringComparison.Ordinal);
            return questionMarkIndex == -1 ? targetQueryString : targetQueryString.Substring(0, questionMarkIndex);
        }

        private string GetFileContent(PageMapping row)
        {
            var getFileClient = CreateClient();
            var downloadDirectory = GetDownloadDirectory();

            var filePath = Path.Combine(downloadDirectory, $"{row.DataFileId}.xml");
            string xml;

            if (File.Exists(filePath))
            {
                _logger.Debug($"Reading file from cache: {filePath}");
                xml = File.ReadAllText(filePath);
            }
            else
            {
                _logger.Debug($"Downloading file to cache: {filePath}");
                var file = getFileClient.GetFileByID(row.PageId, null, new IdcProperty[] { });

                xml = StripByteOrderMark(Encoding.UTF8.GetString(file.downloadFile.fileContent));
                File.WriteAllText(filePath, xml);
            }

            return xml;
        }

        private static string StripByteOrderMark(string value)
        {
            var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

            if (value.StartsWith(byteOrderMarkUtf8))
            {
                value = value.Remove(0, byteOrderMarkUtf8.Length);
            }

            return value;
        }

        private GetFileSoapClient CreateClient()
        {
            return SoapClientFactory.Instance.CreateGetFileSoapClient(_config.GetFileServiceEndpoint, _config.LoginUserName, _config.LoginPassword);
        }

        private string GetDownloadDirectory()
        {
            var downloadDirectory = _config.DownloadFileCacheDirectory;

            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            return downloadDirectory;
        }
    }
}