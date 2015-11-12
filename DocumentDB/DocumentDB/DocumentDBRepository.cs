using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB
{
    public class DocumentDBRepository<T> where T : Document
    {
        private readonly DBClientTemplate<DocumentClient, Database> _client;
        private readonly string _collectionId;
        private readonly string _dbId;

        public DocumentDBRepository()
        {
            IConfig config = new AppSettingsConfig();
            _client = DBClientFactory.GetDocumentDbClient(config);
            _dbId = config.Value<string>("DBName");
            _collectionId = typeof (T).Name;
        }


        public DocumentCollection RetrieveCollection()
        {
            var database = _client.RetrieveDatabase(_dbId);
            var documentCollection =
                _client.GetClient().CreateDocumentCollectionQuery("dbs/" + database.Id)
                    .Where(c => c.Id == _collectionId)
                    .AsEnumerable()
                    .FirstOrDefault();

            // If the document collection does not exist, create a new collection
            if (documentCollection != null) return documentCollection;
            documentCollection = _client.GetClient().CreateDocumentCollectionAsync("dbs/" + database.Id,
                new DocumentCollection
                {
                    Id = _collectionId
                }).Result;

            // Write the new collection's id to the console
            Console.WriteLine(documentCollection.Id);
            return documentCollection;
        }


        public void Create(T t)
        {
            // If the document does not exist, create a new document
            //create your own id..
            var document = _client.GetClient().CreateDocumentAsync("dbs/" + _dbId + "/colls/" + _collectionId, t);
            //  document.Result.Resource.Id;
            Console.WriteLine(document.Result.Resource.SelfLink);
        }

        public void Upsert(T t)
        {
            // If the document does not exist, create a new document
            _client.GetClient().UpsertDocumentAsync("dbs/" + _dbId + "/colls/" + _collectionId, t);
        }

        public void Delete(string docId)
        {
            _client.GetClient().DeleteDocumentAsync("dbs/" + _dbId + "/colls/" + _collectionId + "/docs/" + docId);
        }


        public T GetById(string docId)
        {
            var results =
                from f in _client.GetClient().CreateDocumentQuery<T>("dbs/" + _dbId + "/colls/" + _collectionId)
                where f.Id == docId
                select f;
            return results.FirstOrDefault();
        }

        public IList<T> Get(string sqlQuery)
        {
            var results =
                _client.GetClient().CreateDocumentQuery("dbs/" + _dbId + "/colls/" + _collectionId, sqlQuery);
            IList<T> resultList = new List<T>();
            foreach (var result in results)
            {
                resultList.Add(result);
            }
            return resultList;
        }
    }
}