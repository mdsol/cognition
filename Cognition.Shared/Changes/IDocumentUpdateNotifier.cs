using System.Threading.Tasks;

namespace Cognition.Shared.Changes
{
    public interface IDocumentUpdateNotifier
    {
        Task DocumentUpdated(DocumentChangeNotification notification);
    }
}