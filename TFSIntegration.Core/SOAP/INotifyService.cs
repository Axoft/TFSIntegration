using System.ServiceModel;

namespace TFSIntegration.Core.SOAP
{
    [ServiceContract(Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03")]
    public interface INotifyService
    {
        [OperationContract(Action = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03/Notify")]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void Notify(string eventXml, string tfsIdentityXml);
    }
}
