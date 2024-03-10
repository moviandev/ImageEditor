using ImageEditor.Business.Models;

namespace ImageEditor.Business.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<IEnumerable<Image>> GetAllImagesAsync(Guid id);
        public Task<User> GetUserByEmailAsync(string email);
    }
}