using SmartTaskPro.DTOs;

namespace SmartTaskPro.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<List<UserDto>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int id);
    }
}
