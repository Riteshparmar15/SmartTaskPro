<<<<<<< HEAD
﻿using SmartTaskPro.DTOs;
using System.Security.Claims;

namespace SmartTaskPro.Services
{
    public interface ITaskService
    {
        Task<TaskDto> GetByIdAsync(int id, ClaimsPrincipal currentUser);
        Task<List<TaskDto>> GetAllAsync(ClaimsPrincipal currentUser, int page = 1, int pageSize = 20);
        Task<TaskDto> CreateAsync(CreateTaskDto dto, ClaimsPrincipal currentUser);
        Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto, ClaimsPrincipal currentUser);
        Task DeleteAsync(int id, ClaimsPrincipal currentUser);
=======
﻿namespace SmartTaskPro.Services
{
    public interface ITaskService
    {
>>>>>>> cf439325d2739b097b26b01e22d377ce2a2fa18b
    }
}
