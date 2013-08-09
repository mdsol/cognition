using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Changes;

namespace Cognition.Web.Tests.Mocks
{
    public class MockDocumentUpdateNotifier : IDocumentUpdateNotifier
    {
        public async Task DocumentUpdated(DocumentChangeNotification notification)
        {
          
        }
    }
}
