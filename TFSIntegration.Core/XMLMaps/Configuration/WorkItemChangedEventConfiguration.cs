using System.Collections.Generic;
using System.Xml.Serialization;

namespace TFSIntegration.Core.XMLMaps.Configuration
{
    /// <summary>
    /// 
    /// TEMPLATE.xml
    /// 
    /// <Configuration>
    ///   <EventTypes>
    ///     <EventType WorkItemType="XXX" PortfolioProject="YYY">
    ///       <Tags>
    ///         <Tag Value="">
    ///         </Tag>
    ///         <Tag Value="DEFAULT">
    ///         </Tag>
    ///         <Tag Value="ZZZ">
    ///           <Items>
    ///             <Item>
    ///               <Title></Title>
    ///               <Tag></Tag>
    ///               <AssignTo></AssignTo>
    ///               <ItemType>Task</ItemType>
    ///             </Item>
    ///             <Item>
    ///               <Title></Title>
    ///               <Tag></Tag>
    ///               <AssignTo></AssignTo>
    ///               <ItemType>Task</ItemType>
    ///             </Item>
    ///           </Items>
    ///         </Tag>
    ///       </Tags>
    ///     </EventType>
    ///   </EventTypes>
    /// </Configuration>
    ///
    /// </summary>
    
    [XmlRoot(ElementName = "Tag")]
    public class WorkItemChangedEventTag
    {
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "Items")]
        public WorkItemChangedEventItems Items { get; set; }
    }

    [XmlRoot(ElementName = "Item")]
    public class WorkItemChangedEventItem
    {
        [XmlElement(ElementName = "Title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "Tag")]
        public string Tag { get; set; }
        [XmlElement(ElementName = "AssignTo")]
        public string AssignTo { get; set; }
        [XmlElement(ElementName = "ItemType")]
        public string ItemType { get; set; }
    }

    [XmlRoot(ElementName = "Items")]
    public class WorkItemChangedEventItems
    {
        [XmlElement(ElementName = "Item")]
        public List<WorkItemChangedEventItem> Item { get; set; }
    }

    [XmlRoot(ElementName = "Tags")]
    public class WorkItemChangedEventTags
    {
        [XmlElement(ElementName = "Tag")]
        public List<WorkItemChangedEventTag> Tag { get; set; }
    }

    [XmlRoot(ElementName = "EventType")]
    public class WorkItemChangedEventEventType
    {
        [XmlElement(ElementName = "Tags")]
        public WorkItemChangedEventTags Tags { get; set; }
        [XmlAttribute(AttributeName = "WorkItemType")]
        public string WorkItemType { get; set; }
        [XmlAttribute(AttributeName = "PortfolioProject")]
        public string PortfolioProject { get; set; }
    }

    [XmlRoot(ElementName = "EventTypes")]
    public class WorkItemChangedEventEventTypes
    {
        [XmlElement(ElementName = "EventType")]
        public List<WorkItemChangedEventEventType> EventType { get; set; }
    }

    [XmlRoot(ElementName = "Configuration")]
    public class WorkItemChangedEventConfiguration
    {
        [XmlElement(ElementName = "EventTypes")]
        public WorkItemChangedEventEventTypes EventTypes { get; set; }
    }

}
