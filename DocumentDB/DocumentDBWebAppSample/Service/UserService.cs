using DocumentDBWebAppSample.Domain;
using DocumentDBWebAppSample.Repository;

namespace DocumentDBWebAppSample.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public void Create(User user)
        {
            _userRepository.Create(user);
        }

        public void Update(User user)
        {
            _userRepository.Upsert(user);
        }

        public User Get(string id)
        {
            return _userRepository.GetById(id);
        }
    }
}