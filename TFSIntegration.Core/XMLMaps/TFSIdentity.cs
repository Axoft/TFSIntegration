using System.Xml.Serialization;

namespace TFSIntegration.Core.XMLMaps
{
    [XmlRoot(ElementName = "TeamFoundationServer")]
    public class TFSIdentity
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
    }
}
