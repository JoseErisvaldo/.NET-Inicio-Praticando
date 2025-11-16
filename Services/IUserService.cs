using MinhaApi.Entities;

namespace MinhaApi.Services;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User> Create(UserDto dto);
    Task<User?> Update(Guid id, UserDto dto);
    Task<bool> Delete(Guid id);
}
