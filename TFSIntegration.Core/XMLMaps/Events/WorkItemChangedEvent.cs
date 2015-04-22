using System.Collections.Generic;
using System.Xml.Serialization;

namespace TFSIntegration.Core.XMLMaps.Events
{
    [XmlRoot(ElementName = "Field")]
    public class Field
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "ReferenceName")]
        public string ReferenceName { get; set; }
        [XmlElement(ElementName = "OldValue")]
        public string OldValue { get; set; }
        [XmlElement(ElementName = "NewValue")]
        public string NewValue { get; set; }
    }

    [XmlRoot(ElementName = "IntegerFields")]
    public class IntegerFields
    {
        [XmlElement(ElementName = "Field")]
        public List<Field> Field { get; set; }
    }

    [XmlRoot(ElementName = "StringFields")]
    public class StringFields
    {
        [XmlElement(ElementName = "Field")]
        public List<Field> Field { get; set; }
    }

    [XmlRoot(ElementName = "CoreFields")]
    public class CoreFields
    {
        [XmlElement(ElementName = "IntegerFields")]
        public IntegerFields IntegerFields { get; set; }
        [XmlElement(ElementName = "StringFields")]
        public StringFields StringFields { get; set; }
    }

    [XmlRoot(ElementName = "TextField")]
    public class TextField
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "ReferenceName")]
        public string ReferenceName { get; set; }
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "TextFields")]
    public class TextFields
    {
        [XmlElement(ElementName = "TextField")]
        public List<TextField> TextField { get; set; }
    }

    [XmlRoot(ElementName = "ChangedFields")]
    public class ChangedFields
    {
        [XmlElement(ElementName = "IntegerFields")]
        public IntegerFields IntegerFields { get; set; }
        [XmlElement(ElementName = "StringFields")]
        public StringFields StringFields { get; set; }
    }

    [XmlRoot(ElementName = "AddedRelation")]
    public class AddedRelation
    {
        [XmlElement(ElementName = "LinkName")]
        public string LinkName { get; set; }
        [XmlElement(ElementName = "WorkItemId")]
        public string WorkItemId { get; set; }
    }

    [XmlRoot(ElementName = "AddedRelations")]
    public class AddedRelations
    {
        [XmlElement(ElementName = "AddedRelation")]
        public AddedRelation AddedRelation { get; set; }
    }

    [XmlRoot(ElementName = "WorkItemChangedEvent")]
    public class WorkItemChangedEvent : TFSEvent
    {
        [XmlElement(ElementName = "PortfolioProject")]
        public string PortfolioProject { get; set; }
        [XmlElement(ElementName = "ProjectNodeId")]
        public string ProjectNodeId { get; set; }
        [XmlElement(ElementName = "AreaPath")]
        public string AreaPath { get; set; }
        [XmlElement(ElementName = "Title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "WorkItemTitle")]
        public string WorkItemTitle { get; set; }
        [XmlElement(ElementName = "Subscriber")]
        public string Subscriber { get; set; }
        [XmlElement(ElementName = "ChangerSid")]
        public string ChangerSid { get; set; }
        [XmlElement(ElementName = "ChangerTeamFoundationId")]
        public string ChangerTeamFoundationId { get; set; }
        [XmlElement(ElementName = "DisplayUrl")]
        public string DisplayUrl { get; set; }
        [XmlElement(ElementName = "TimeZone")]
        public string TimeZone { get; set; }
        [XmlElement(ElementName = "TimeZoneOffset")]
        public string TimeZoneOffset { get; set; }
        [XmlElement(ElementName = "ChangeType")]
        public string ChangeType { get; set; }
        [XmlElement(ElementName = "CoreFields")]
        public CoreFields CoreFields { get; set; }
        [XmlElement(ElementName = "TextFields")]
        public TextFields TextFields { get; set; }
        [XmlElement(ElementName = "ChangedFields")]
        public ChangedFields ChangedFields { get; set; }
        [XmlElement(ElementName = "AddedRelations")]
        public AddedRelations AddedRelations { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }
}
