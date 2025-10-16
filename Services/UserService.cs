using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;
using SmartTaskPro.Repositories;

namespace SmartTaskPro.Services
{   
    
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _hasher;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            var users = await _repo.Query()
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var existing = await _repo.GetByEmail(dto.Email);
            if (existing != null) throw new ApplicationException("Email exists");

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Role = Enum.TryParse<Role>(dto.Role, true, out var r) ? r : Role.User
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new ApplicationException("Not found");

            if (!string.IsNullOrWhiteSpace(dto.FullName)) user.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Role) && Enum.TryParse<Role>(dto.Role, true, out var r)) user.Role = r;

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new ApplicationException("Not found");
            user.IsDeleted = true;
            _repo.Update(user);
            await _repo.SaveChangesAsync();
        }
    }
}
