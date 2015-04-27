using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TFSIntegration.Core.Const;
using TFSIntegration.Core.XMLMaps;
using TFSIntegration.Core.XMLMaps.Configuration;
using TFSIntegration.Core.XMLMaps.Events;

namespace TFSIntegration.Core.EventHandlers
{
    class CreatedTaskInfo
    {
        public string CreatedTaskID { get; set; }
        public string CreatedTaskArea { get; set; }
        public string CreatedTaskIteration { get; set; }
        public string CreatedTaskType { get; set; }
        public string CreatedTaskPortfolio { get; set; }
    }

    public class WorkItemChangedEventHandler : TFSEventHandler<WorkItemChangedEvent>
    {
        #region Private members
        private WorkItemChangedEventConfiguration GetEventConfiguration()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, WorkItemChangedEventConst.CONFIG_FILE);

            if (!File.Exists(filename))
            {
                throw new Exception("Configuration file not found");
            }

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                XmlSerializer configurationSerializer = new XmlSerializer(typeof(WorkItemChangedEventConfiguration));
                return (WorkItemChangedEventConfiguration)configurationSerializer.Deserialize(fs);
            }
        }

        private void ThrowIfFieldNotFound(XMLMaps.Events.Field field, string fieldName)
        {
            if (field == null)
            {
                throw new Exception(string.Format("Field not found: {0}", fieldName));
            };
        }

        private string GetIntegerFieldValue(WorkItemChangedEvent eventInfo, string fieldName)
        {
            XMLMaps.Events.Field field = eventInfo.CoreFields.IntegerFields.Field
                .Where(f => f.ReferenceName == fieldName)
                .FirstOrDefault();

            ThrowIfFieldNotFound(field, fieldName);
            return field.NewValue;
        }

        private string GetStringFieldValue(WorkItemChangedEvent eventInfo, string fieldName)
        {
            XMLMaps.Events.Field field = eventInfo.CoreFields.StringFields.Field
                .Where(f => f.ReferenceName == fieldName)
                .FirstOrDefault();

            ThrowIfFieldNotFound(field, fieldName);
            return field.NewValue;
        }

        private IList<string> GetTags(WorkItemChangedEvent eventInfo)
        {
            List<string> tags = new List<string>();
            if (eventInfo.TextFields != null)
            {
                var tagField = eventInfo.TextFields.TextField.Where(f => f.Name == "Tags").FirstOrDefault();
                if (tagField != null)
                {
                    tags.AddRange(tagField.Value.Split(';'));
                    tags.ForEach(t => t = t.Trim());
                }   
            }
            return tags;
        }

        private string GetMainArea(string area)
        {
            return area.Split('\\')[0];
        }

        private WorkItemStore GetWorkItemStore(TFSIdentity itentity)
        {
            Uri tfsUri = new Uri(itentity.Url);
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(string.Format(TFSServerConsts.HOST_PATTERN, tfsUri.Host, tfsUri.Segments[2])), base.GetCredentials());

            return (WorkItemStore)tfs.GetService(typeof(WorkItemStore));
        }

        private void ProcessTag(WorkItemChangedEventTag tag, WorkItemTypeCollection workItemTypes, WorkItemLinkTypeEnd linkTypeEnd, CreatedTaskInfo createdTaskInfo)
        {
            if (tag == null)
            {
                EventManager.Notify(EventType.Error, string.Format("Tag {0} has not configuration", tag));
            }
            else
            {
                foreach (WorkItemChangedEventItem item in tag.Items.Item)
                {
                    CreateItem(workItemTypes, linkTypeEnd, createdTaskInfo, item);
                }
            }
        }

        private void CreateItem(WorkItemTypeCollection workItemTypes, WorkItemLinkTypeEnd linkTypeEnd, CreatedTaskInfo createdTaskInfo, WorkItemChangedEventItem item)
        {
            WorkItemType workItemType = workItemTypes[item.ItemType];
            if (workItemType == null)
            {
                EventManager.Notify(EventType.Error, string.Format("No supported item type {0}", item.ItemType));
            }
            else
            {
                WorkItem workItem = new WorkItem(workItemType);
                workItem.Fields[TFSFieldsReferenceNameConsts.TITLE].Value = item.Title;
                workItem.Fields[TFSFieldsReferenceNameConsts.AREA].Value = createdTaskInfo.CreatedTaskArea;
                workItem.Fields[TFSFieldsReferenceNameConsts.ITERATION].Value = createdTaskInfo.CreatedTaskIteration;
                workItem.Fields[TFSFieldsReferenceNameConsts.STATE].Value = TFSTaskStateConsts.NEW;
                if (!string.IsNullOrWhiteSpace(item.Tag))
                {
                    workItem.Tags = item.Tag;
                }

                if (!string.IsNullOrWhiteSpace(item.AssignTo))
                {
                    workItem.Fields[TFSFieldsReferenceNameConsts.ASSIGNED_TO].Value = item.AssignTo;
                }

                RelatedLink link = new RelatedLink(linkTypeEnd, Convert.ToInt32(createdTaskInfo.CreatedTaskID));
                workItem.Links.Add(link);

                ArrayList validationErrors = workItem.Validate();

                if (validationErrors.Count == 0)
                {
                    workItem.Save();
                    EventManager.Notify(EventType.Info, string.Format("Generated Task ID: {0}", workItem.Id));
                }
                else
                {
                    foreach (Microsoft.TeamFoundation.WorkItemTracking.Client.Field field in validationErrors)
                    {
                        EventManager.Notify(EventType.Error, string.Format("Error {0}:{1}", field.ReferenceName, field.Status));
                    }
                }
            }
        }

        private CreatedTaskInfo GetTaskInfoFromEventInfo(WorkItemChangedEvent eventInfo)
        {
            string createdTaskID = GetIntegerFieldValue(eventInfo, TFSFieldsReferenceNameConsts.ID);
            string createdTaskArea = GetStringFieldValue(eventInfo, TFSFieldsReferenceNameConsts.AREA);
            string createdTaskIteration = GetStringFieldValue(eventInfo, TFSFieldsReferenceNameConsts.ITERATION);
            string createdTaskType = GetStringFieldValue(eventInfo, TFSFieldsReferenceNameConsts.WORK_ITEM_TYPE);

            createdTaskArea = createdTaskArea.StartsWith(@"\") ? createdTaskArea.Substring(1) : createdTaskArea;
            createdTaskIteration = createdTaskIteration.StartsWith(@"\") ? createdTaskIteration.Substring(1) : createdTaskIteration;

            CreatedTaskInfo createdTaskInfo = new CreatedTaskInfo
            {
                CreatedTaskArea = createdTaskArea,
                CreatedTaskID = createdTaskID,
                CreatedTaskIteration = createdTaskIteration,
                CreatedTaskType = createdTaskType,
                CreatedTaskPortfolio = eventInfo.PortfolioProject
            };

            return createdTaskInfo;
        }
        #endregion

        protected override void CustomProcessEvent(WorkItemChangedEvent eventInfo, TFSIdentity itentity)
        {
            WorkItemStore workItemStore = GetWorkItemStore(itentity);
            WorkItemTypeCollection workItemTypes = workItemStore.Projects[eventInfo.PortfolioProject].WorkItemTypes;
            WorkItemLinkTypeEnd linkTypeEnd = workItemStore.WorkItemLinkTypes.LinkTypeEnds[TFSLinkTypesConsts.PARENT];

            WorkItemChangedEventConfiguration configuration = GetEventConfiguration();

            CreatedTaskInfo createdTaskInfo = GetTaskInfoFromEventInfo(eventInfo);

            IList<string> tags = GetTags(eventInfo);

            WorkItemChangedEventEventType eventType = configuration.EventTypes.EventType
                .Where(e => e.PortfolioProject.ToUpper() == createdTaskInfo.CreatedTaskPortfolio.ToUpper()
                    && e.WorkItemType.ToUpper() == createdTaskInfo.CreatedTaskType.ToUpper())
                .FirstOrDefault();

            if (eventType != null)
            {
                
                WorkItemChangedEventTag tagDefault = eventType.Tags.Tag.Where(t => t.Value == WorkItemChangedEventConst.TAG_DEFAULT).FirstOrDefault();
                ProcessTag(tagDefault, workItemTypes, linkTypeEnd, createdTaskInfo);

                if (tags.Count == 0)
                {
                    WorkItemChangedEventTag tagEmpty = eventType.Tags.Tag.Where(t => t.Value == WorkItemChangedEventConst.TAG_EMPTY).FirstOrDefault();
                    ProcessTag(tagEmpty, workItemTypes, linkTypeEnd, createdTaskInfo);
                }

                foreach (string tag in tags)
                {
                    WorkItemChangedEventTag tagConfiguration = eventType.Tags != null ? eventType.Tags.Tag.Where(t => t.Value == tag).FirstOrDefault() : null;
                    ProcessTag(tagConfiguration, workItemTypes, linkTypeEnd, createdTaskInfo);
                }
            }
        }
    }
}
