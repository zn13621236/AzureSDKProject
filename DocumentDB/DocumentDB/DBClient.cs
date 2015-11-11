using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB
{
    public class DBClient
    {
        private DocumentClient _dbClient;
        private DBConfig _dbConfig;

        public DBClient(IConfig config)
        {
            this._dbConfig = new DBConfig(config);
            this._dbClient = new DocumentClient(new Uri(_dbConfig.Url), _dbConfig.Key);
        }

        protected Database RetrieveDatabase(string dbId)
        {
            var database =
                _dbClient.CreateDatabaseQuery().Where(db => db.Id == dbId).AsEnumerable().FirstOrDefault();

            // If the database does not exist, create a new database
            if (database != null) return database;
            database = _dbClient.CreateDatabaseAsync(
                new Database
                {
                    Id = dbId
                }).Result;

            // Write the new database's id to the console
            Console.WriteLine(database.Id);
            return database;
        }
        private DocumentCollection RetrieveCollection(string dbId, string collectionId)
        {
            var database = RetrieveDatabase(dbId);
            var documentCollection =
                _dbClient.CreateDocumentCollectionQuery("dbs/" + database.Id)
                    .Where(c => c.Id == collectionId)
                    .AsEnumerable()
                    .FirstOrDefault();

            // If the document collection does not exist, create a new collection
            if (documentCollection != null) return documentCollection;
            documentCollection = _dbClient.CreateDocumentCollectionAsync("dbs/" + database.Id,
                new DocumentCollection
                {
                    Id = collectionId
                }).Result;

            // Write the new collection's id to the console
            Console.WriteLine(documentCollection.Id);
            return documentCollection;
        }




        private void Create<T>(string dbId, string collectionId, T t)
        {
            // If the document does not exist, create a new document
            //create your own id..
            var document = _dbClient.CreateDocumentAsync("dbs/" + dbId + "/colls/" + collectionId, t);
            //  document.Result.Resource.Id;
            Console.WriteLine(document.Result.Resource.SelfLink);
        }

        private void Upsert<T>(string dbId, string collectionId, T t)
        {
            // If the document does not exist, create a new document
            _dbClient.UpsertDocumentAsync("dbs/" + dbId + "/colls/" + collectionId, t);
        }

        private void Delete(string dbId, string collectionId, string docId)
        {
            _dbClient.DeleteDocumentAsync("dbs/" + dbId + "/colls/" + collectionId + "/docs/" + docId);
        }


        private T GetById<T>(string dbId, string collectionId, string docId) where T : Document
        {
            var results =
                from f in _dbClient.CreateDocumentQuery<T>("dbs/" + dbId + "/colls/" + collectionId)
                where f.Id == docId
                select f;
            return results.FirstOrDefault();
        }

        private IList<T> Get<T>(string dbId, string collectionId, string sqlQuery) where T : Document
        {
            var results =
                _dbClient.CreateDocumentQuery("dbs/" + dbId + "/colls/" + collectionId, sqlQuery);
            IList<T> resultList = new List<T>();
            foreach (var result in results)
            {
                resultList.Add(result);
            }
            return resultList;
        }
    }
}
