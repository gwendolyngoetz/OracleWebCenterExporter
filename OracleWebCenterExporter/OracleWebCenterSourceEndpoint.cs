using System.Diagnostics;
using System.Linq;
using NLog;
using OracleWebCenterExporter.Model;
using OracleWebCenterExporter.Services;
using OracleWebCenterExporter.XmlDeserialization;

namespace OracleWebCenterExporter
{
    public class OracleWebCenterSourceEndpoint
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly WebCenterConfiguration _config;

        public OracleWebCenterSourceEndpoint(WebCenterConfiguration config)
        {
            _config = config;
        }

        public WebCenterPayload Execute()
        {
            var fullName = GetType().FullName;
            var stopwatch = Stopwatch.StartNew();
           
            try
            {
                _logger.Info($"Start - {fullName}");

                var pageMetadata = new WebCenterMetadataService(_config).GetPageMetadata();

                var xmlDeserializer = new WebCenterXmlDeserializer();
                var pages = pageMetadata.PageMappings.Select(page => xmlDeserializer.Deserialize(page)).ToList();

                return new WebCenterPayload
                {
                    SiteHierarchyTree = pageMetadata.SiteHierarchyTree,
                    Assets = pageMetadata.Assets,
                    Pages = pages
                };
            }
            catch
            {
                _logger.Error($"Error - {fullName}");
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _logger.Info($"End - {fullName}. (Elapsed: {stopwatch.ElapsedMilliseconds}ms)");
            }
        }
    }
}
