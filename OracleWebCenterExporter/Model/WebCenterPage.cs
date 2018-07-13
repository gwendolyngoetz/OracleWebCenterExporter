using System;
using System.Collections.Generic;
using System.Linq;

namespace OracleWebCenterExporter.Model
{
    public class WebCenterPage
    {
        public List<WebCenterElement> Elements { get; } = new List<WebCenterElement>();

        public void AddElement(WebCenterElement webCenterElement)
        {
            Elements.Add(webCenterElement);
        }

        public string GetElementValue(string placeholderName, string name)
        {
            var element = Elements.FirstOrDefault(x => x.Name == name && x.PlaceholderName == placeholderName);

            if (element == null)
            {
                throw new ArgumentNullException($"{nameof(Elements)} does not contain a item matching \"name: {name}, placeholderName: {placeholderName}\".");
            }

            return element.Value;
        }

        public List<WebCenterElement> GetElements(string placeholderName, string name)
        {
            return Elements.Where(x => x.Name == name && x.PlaceholderName == placeholderName).ToList();
        }
    }
}
