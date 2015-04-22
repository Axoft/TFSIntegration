
namespace TFSIntegration.Core.Const
{
    static class TFSServerConsts
    {
        public const string HOST_PATTERN = "http://{0}:8080/tfs/{1}";
    }
    static class TFSFieldsReferenceNameConsts
    {
        public const string TITLE = "System.Title";
        public const string AREA = "System.AreaPath";
        public const string ITERATION = "System.IterationPath";
        public const string STATE = "System.State";
        public const string ASSIGNED_TO = "System.AssignedTo";
        public const string ID = "System.Id";
        public const string WORK_ITEM_TYPE = "System.WorkItemType";
    }

    static class TFSLinkTypesConsts
    {
        public const string PARENT = "Parent";
    }

    static class TFSTaskStateConsts
    {
        public const string NEW = "New";
    }
}
