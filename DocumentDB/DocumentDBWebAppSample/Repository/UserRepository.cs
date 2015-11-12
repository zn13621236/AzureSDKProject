using DocumentDB;
using DocumentDBWebAppSample.Domain;

namespace DocumentDBWebAppSample.Repository
{
    public class UserRepository : DocumentDBRepository<User>, IUserRepository
    {
    }

    public interface IUserRepository
    {
    }
}