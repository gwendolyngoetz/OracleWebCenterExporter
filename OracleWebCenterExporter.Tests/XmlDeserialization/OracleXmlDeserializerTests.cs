using System.Collections.Generic;
using System.Text.RegularExpressions;
using OracleWebCenterExporter.XmlDeserialization;
using Shouldly;
using Xunit;

namespace OracleWebCenterExporter.Tests.XmlDeserialization
{
    public class OracleXmlDeserializerTests
    {
        #region Xml Data

        private const string _xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<wcm:root xmlns:wcm=""http://www.stellent.com/wcm-data/ns/8.0.0"" version=""8.0.0.0"">
    <wcm:element name=""Heading"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">Heading1</wcm:element>
    <wcm:element name=""Image"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">Image1</wcm:element>
    <wcm:element name=""ImageText"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">ImageText1</wcm:element>
    <wcm:element name=""ContentArea"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">ContentArea1</wcm:element>
    <wcm:element name=""Body"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">Body1</wcm:element>
    <wcm:list name=""list1"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">
        <wcm:row name=""row1"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">
            <wcm:element name=""element1"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">Element1Content</wcm:element>
            <wcm:element name=""element2"" dataFileId=""1"" pageId="""" placeholderName="""" label="""" nodeId="""" url="""" pageTemplateName="""">Element2Content</wcm:element>
        </wcm:row>    
    </wcm:list>
</wcm:root>";

        #endregion

        [Fact]
        public void heading_exists()
        {
            var deserializer = new WebCenterXmlDeserializer();
            var result = deserializer.Deserialize(_xml);
            result.GetElementValue("", "Heading").ShouldBe("Heading1");
        }

        [Fact]
        public void image_exists()
        {
            var deserializer = new WebCenterXmlDeserializer();
            var result = deserializer.Deserialize(_xml);
            result.GetElementValue("", "Image").ShouldBe("Image1");
        }

        [Fact]
        public void image_text_exists()
        {
            var deserializer = new WebCenterXmlDeserializer();
            var result = deserializer.Deserialize(_xml);
            result.GetElementValue("", "ImageText").ShouldBe("ImageText1");
        }

        [Fact]
        public void content_area_exists()
        {
            var deserializer = new WebCenterXmlDeserializer();
            var result = deserializer.Deserialize(_xml);
            result.GetElementValue("", "ContentArea").ShouldBe("ContentArea1");
        }

        [Fact]
        public void body_exists()
        {
            var deserializer = new WebCenterXmlDeserializer();
            var result = deserializer.Deserialize(_xml);
            result.GetElementValue("", "Body").ShouldBe("Body1");
        }
    }
}
