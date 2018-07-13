using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using OracleWebCenterExporter.DocInfoSvc;
using OracleWebCenterExporter.Extensions;
using OracleWebCenterExporter.Infrastructure;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.Services
{
    internal class WebCenterAssetService
    {
        private const string _dataFileName = "Metadata\\DataFileList.json";

        protected readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly WebCenterConfiguration _config;

        public WebCenterAssetService(WebCenterConfiguration config)
        {
            _config = config;
        }

        public List<WebCenterAsset> GetAssets(SiteReportResultSet siteReportResultSet)
        {
            var dataFilePath = GetDataFilePath();

            if (File.Exists(dataFilePath))
            {
                return JsonConvert.DeserializeObject<List<WebCenterAsset>>(File.ReadAllText(dataFilePath));
            }

            var list = new ConcurrentBag<WebCenterAsset>();

            var count = 0;
            FilterWebsiteDocs(siteReportResultSet.WebsiteDocs).Each(oraclePageMapping =>
            {
                count++;
                var result = DownloadFileMetadata(oraclePageMapping);

                if (result == null)
                {
                    return;
                }

                list.Add(result);

                _logger.Info($"{count}: Downloaded file metadata: {result.DataFileId} ({result.FileName})");
            });

            File.WriteAllText(dataFilePath, JsonConvert.SerializeObject(list, Formatting.Indented, CustomJsonSerializerSettings.Instance.Settings));

            return list.ToList();
        }

        private string GetDataFilePath()
        {
            var filePath = Path.Combine(_config.DownloadFileCacheDirectory, _dataFileName);
            var path = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return filePath;
        }

        private List<WebsiteDocs> FilterWebsiteDocs(List<WebsiteDocs> websiteDocs)
        {
            var results = websiteDocs
                .Where(x => (x.XWebsiteObjectType.IsNullOrWhiteSpace() ? "null" : x.XWebsiteObjectType).In("Native Document", "Image", "null"))
                .ToList();

            _logger.Info($"Total files to process: {websiteDocs.Count}");

            return results;
        }

        private DocInfoSoapClient CreateClient()
        {
            return SoapClientFactory.Instance.CreateDocInfoSoapClient(_config.GetFileServiceEndpoint, _config.LoginUserName, _config.LoginPassword);
        }

        private WebCenterAsset DownloadFileMetadata(WebsiteDocs oraclePageMapping)
        {
            var docInfoClient = CreateClient();
            var docInfo = docInfoClient.DocInfoByID(oraclePageMapping.PageId, new IdcProperty[] { });
            var contentInfo = docInfo.ContentInfo.FirstOrDefault();

            if (contentInfo == null)
            {
                return null;
            }
            
            return new WebCenterAsset
            {
                DataFileId = oraclePageMapping.DataFileId,
                PageId = oraclePageMapping.PageId,
                WebsiteObjectType = oraclePageMapping.XWebsiteObjectType,
                FileName = contentInfo.dOriginalName,
                DocumentName = contentInfo.dDocTitle,
                FileExtension = contentInfo.dExtension
            };
        }

    }
}