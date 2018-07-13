using System.Configuration;

namespace OracleWebCenterExporter.Model
{
    public class WebCenterConfiguration
    {
        public string LoginUserName { get; set; }
        public string LoginPassword { get; set; }
        public string LoginEndpoint { get; set; }
        public string SiteReportEndpoint { get; set; }
        public string GetFileServiceEndpoint { get; set; }
        public string DownloadFileCacheDirectory { get; set; }

        public WebCenterConfiguration()
        {
            LoginUserName = ConfigurationManager.AppSettings["LoginUserName"];
            LoginPassword = ConfigurationManager.AppSettings["LoginPassword"];
            LoginEndpoint = ConfigurationManager.AppSettings["LoginEndpoint"];
            SiteReportEndpoint = ConfigurationManager.AppSettings["SiteReportEndpoint"];
            GetFileServiceEndpoint = ConfigurationManager.AppSettings["GetFileServiceEndpoint"];
            DownloadFileCacheDirectory = ConfigurationManager.AppSettings["DownloadFileCacheDirectory"];
        }
    }
}
