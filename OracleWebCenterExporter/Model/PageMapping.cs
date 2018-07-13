namespace OracleWebCenterExporter.Model
{
    internal class PageMapping
    {
        public string DataFileId { get; set; }
        public int PageId { get; set; }
        public int NodeId { get; set; }
        public string Label { get; set; }
        public string NamePath { get; set; }
        public string TargetQueryString { get; set; }
        public string PlaceholderName { get; set; }
        public string PageTemplateName { get; set; }
        public string Xml { get; set; }
    }
}