using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using TestDocumentDB.Model;

namespace TestDocumentDB
{
    class DBRepo
    {
        private DocumentClient client;
        private const string EndpointUrl = "";
        private const string AuthorizationKey = "";

        public DBRepo()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey); ;

        }



        public  void GetStartedDemo()
        {
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == "FamilyRegistry").AsEnumerable().FirstOrDefault();

            // If the database does not exist, create a new database
            if (database == null)
            {
                database =  client.CreateDatabaseAsync(
                    new Database
                    {
                        Id = "FamilyRegistry"
                    }).Result;

                // Write the new database's id to the console
                Console.WriteLine(database.Id);
                Console.WriteLine("Press any key to continue ...");
                Console.ReadKey();
                Console.Clear();
            }

            // Check to verify a document collection with the id=FamilyCollection does not exist
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery("dbs/" + database.Id).Where(c => c.Id == "FamilyCollection").AsEnumerable().FirstOrDefault();

            // If the document collection does not exist, create a new collection
            if (documentCollection == null)
            {
                documentCollection =  client.CreateDocumentCollectionAsync("dbs/" + database.Id,
                    new DocumentCollection
                    {
                        Id = "FamilyCollection"
                    }).Result;

                // Write the new collection's id to the console
                Console.WriteLine(documentCollection.Id);
                Console.WriteLine("Press any key to continue ...");
                Console.ReadKey();
                Console.Clear();
            }



            // Check to verify a document with the id=AndersenFamily does not exist
            Document document = client.CreateDocumentQuery("dbs/" + database.Id + "/colls/" + documentCollection.Id).Where(d => d.Id == "AndersenFamily").AsEnumerable().FirstOrDefault();

            // If the document does not exist, create a new document
            if (document == null)
            {
                // Create the Andersen Family document
                Family andersonFamily = new Family
                {
                    Id = "AndersenFamily",
                    LastName = "Andersen",
                    Parents = new Parent[] {
            new Parent { FirstName = "Thomas" },
            new Parent { FirstName = "Mary Kay"}
        },
                    Children = new Child[] {
            new Child
            {
                FirstName = "Henriette Thaulow",
                Gender = "female",
                Grade = 5,
                Pets = new Pet[] {
                    new Pet { GivenName = "Fluffy" }
                }
            }
        },
                    Address = new Address { State = "WA", County = "King", City = "Seattle" },
                    IsRegistered = true
                };

                // id based routing for the first argument, "dbs/FamilyRegistry/colls/FamilyCollection"
                 client.CreateDocumentAsync("dbs/" + database.Id + "/colls/" + documentCollection.Id, andersonFamily);
            }

            // Check to verify a document with the id=AndersenFamily does not exist
            document = client.CreateDocumentQuery("dbs/" + database.Id + "/colls/" + documentCollection.Id).Where(d => d.Id == "WakefieldFamily").AsEnumerable().FirstOrDefault();

            if (document == null)
            {
                // Create the WakeField document
                Family wakefieldFamily = new Family
                {
                    Id = "WakefieldFamily",
                    Parents = new Parent[] {
            new Parent { FamilyName= "Wakefield", FirstName= "Robin" },
            new Parent { FamilyName= "Miller", FirstName= "Ben" }
        },
                    Children = new Child[] {
            new Child {
                FamilyName= "Merriam",
                FirstName= "Jesse",
                Gender= "female",
                Grade= 8,
                Pets= new Pet[] {
                    new Pet { GivenName= "Goofy" },
                    new Pet { GivenName= "Shadow" }
                }
            },
            new Child {
                FamilyName= "Miller",
                FirstName= "Lisa",
                Gender= "female",
                Grade= 1
            }
        },
                    Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                    IsRegistered = false
                };

                // id based routing for the first argument, "dbs/FamilyRegistry/colls/FamilyCollection"
                 client.CreateDocumentAsync("dbs/" + database.Id + "/colls/" + documentCollection.Id, wakefieldFamily);
            }






            // Query the documents using DocumentDB SQL for the Andersen family.
            var families = client.CreateDocumentQuery("dbs/" + database.Id + "/colls/" + documentCollection.Id,
                "SELECT * " +
                "FROM Families f " +
                "WHERE f.id = \"AndersenFamily\"");

            foreach (var family in families)
            {
                Console.WriteLine("\tRead {0} from SQL", family);
            }

            // Query the documents using LINQ for the Andersen family.
            families =
                from f in client.CreateDocumentQuery("dbs/" + database.Id + "/colls/" + documentCollection.Id)
                where f.Id == "AndersenFamily"
                select f;

            foreach (var family in families)
            {
                Console.WriteLine("\tRead {0} from LINQ", family);
            }

            // Query the documents using LINQ lambdas for the Andersen family.
            families = client.CreateDocumentQuery("dbs/" + database.Id + "/colls/" + documentCollection.Id)
                .Where(f => f.Id == "AndersenFamily")
                .Select(f => f);

            foreach (var family in families)
            {
                Console.WriteLine("\tRead {0} from LINQ query", family);
            }
        }


    }
}
