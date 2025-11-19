using AutoMapper;
using MinhaApi.Entities;

namespace MinhaApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _repo.GetUsersAsync();
    }

    public Task<User?> GetUserByIdAsync(Guid id)
    {
        return _repo.GetUserByIdAsync(id);
    }

    public async Task<User> Create(UserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        await _repo.AddUserAsync(user);
        return user;
    }

    public async Task<User?> Update(Guid id, UserDto dto)
    {
        var user = await _repo.GetUserByIdAsync(id);
        if (user == null) return null;

        _mapper.Map(dto, user);
        await _repo.UpdateAsync(user);
        return user;
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await _repo.GetUserByIdAsync(id);
        if (user == null) return false;

        await _repo.DeleteAsync(user);
        return true;
    }
}
