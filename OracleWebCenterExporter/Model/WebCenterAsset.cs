
namespace OracleWebCenterExporter.Model
{
    public class WebCenterAsset
    {
        public string DataFileId { get; set; }
        public int PageId { get; set; }
        public string WebsiteObjectType { get; set; }
        public string DocumentName { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string DataFileName => $"{DataFileId}.{FileExtension}";
    }
}