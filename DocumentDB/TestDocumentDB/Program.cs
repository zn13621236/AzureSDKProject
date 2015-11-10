using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace TestDocumentDB
{
    class Program
    {
     

        static void Main(string[] args)
        {
           var repo= new DBRepo();
            repo.GetStartedDemo();
        }
    }
}
