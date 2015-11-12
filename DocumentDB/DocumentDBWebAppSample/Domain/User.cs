using Microsoft.Azure.Documents;

namespace DocumentDBWebAppSample.Domain
{
    public class User : Document
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}