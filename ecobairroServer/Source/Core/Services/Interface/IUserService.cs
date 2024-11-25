using ecobairroServer.Data;
using ecobairroServer.Source.Core.Models.Pessoa;

namespace ecobairroServer.Source.Core.Services.Interface
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User?> GetByEmail(string email);
        public Task<User?> GetById(int id);
    }
}
