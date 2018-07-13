using NSubstitute;
using OracleWebCenterExporter.Infrastructure;
using OracleWebCenterExporter.Model;
using OracleWebCenterExporter.Services;
using Shouldly;
using Xunit;

namespace OracleWebCenterExporter.Tests.Services
{
    public class SiteReportRepositoryTests
    {
        [Fact]
        public void can_load_site_report_metadata()
        {
            var config = new WebCenterConfiguration();

            var webClient = Substitute.For<ICookieAwareWebClient>();
            webClient.DownloadString(Arg.Any<string>()).Returns(ResourceFile.SiteReportMetadataJson);

            var repository = new SiteReportService(config, webClient);
            var result = repository.GetSiteReport();
            result.ShouldNotBeNull();

            result.NodeInfo.ShouldNotBeNull();
            result.NodeInfo.ShouldNotBeEmpty();

            result.WebsiteDocs.ShouldNotBeNull();
            result.WebsiteDocs.ShouldNotBeEmpty();

            result.SiteHierarchy.ShouldNotBeNull();
            result.SiteHierarchy.ShouldNotBeEmpty();

            result.UrlDataFiles.ShouldNotBeNull();
            result.UrlDataFiles.ShouldNotBeEmpty();
        }
    }
}