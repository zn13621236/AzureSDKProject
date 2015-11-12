using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DocumentDB
{
    public class DBClientFactory
    {
        public static DBClientTemplate<DocumentClient, Database> GetDocumentDbClient(IConfig config)
        {
            return new DocumentDBClient(config);
        }
    }
}