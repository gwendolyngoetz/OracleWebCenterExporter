
namespace OracleWebCenterExporter.Model
{
    public class WebCenterElement
    {
        public int PageId { get; set; }
        public int NodeId { get; set; }
        public string DataFileId { get; set; }
        public string Name { get; set; }
        public string PlaceholderName { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Url { get; set; }
        public string PageTemplateName { get; set; }
    }
}