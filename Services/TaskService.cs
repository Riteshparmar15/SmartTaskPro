
﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;
using SmartTaskPro.Repositories;
using System.Security.Claims;

namespace SmartTaskPro.Services
{
    
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository repo, IUserRepository userRepo, IMapper mapper)
        {
            _repo = repo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        private int GetCurrentUserId(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            return idClaim != null ? int.Parse(idClaim.Value) : 0;
        }

        private bool IsAdmin(ClaimsPrincipal user) => user.IsInRole(Role.Admin.ToString());
        private bool IsManager(ClaimsPrincipal user) => user.IsInRole(Role.Manager.ToString());

        public async Task<TaskDto> GetByIdAsync(int id, ClaimsPrincipal currentUser)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) throw new ApplicationException("Not found");

            if (IsAdmin(currentUser)) return _mapper.Map<TaskDto>(task);

            var userId = GetCurrentUserId(currentUser);
            if (task.AssignedToUserId != userId) throw new UnauthorizedAccessException();
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<List<TaskDto>> GetAllAsync(ClaimsPrincipal currentUser, int page = 1, int pageSize = 20)
        {
            var query = _repo.Query();

            if (!IsAdmin(currentUser) && !IsManager(currentUser))
            {
                var userId = GetCurrentUserId(currentUser);
                query = query.Where(t => t.AssignedToUserId == userId);
            }
            // Managers: in this simple sample managers see all tasks (you can limit by team)
            var items = await query.OrderByDescending(t => t.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return _mapper.Map<List<TaskDto>>(items);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto, ClaimsPrincipal currentUser)
        {
            var task = _mapper.Map<TaskItem>(dto);
            await _repo.AddAsync(task);
            await _repo.SaveChangesAsync();
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto, ClaimsPrincipal currentUser)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) throw new ApplicationException("Not found");

            // Only admin or assigned user can update (simple rule)
            var userId = GetCurrentUserId(currentUser);
            if (!IsAdmin(currentUser) && task.AssignedToUserId != userId)
                throw new UnauthorizedAccessException();

            if (!string.IsNullOrWhiteSpace(dto.Title)) task.Title = dto.Title;
            if (dto.Description != null) task.Description = dto.Description;
            if (dto.AssignedToUserId.HasValue) task.AssignedToUserId = dto.AssignedToUserId;
            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<Models.TaskStatus>(dto.Status, true, out var s)) task.Status = s;

            _repo.Update(task);
            await _repo.SaveChangesAsync();
            return _mapper.Map<TaskDto>(task);
        }

        public async Task DeleteAsync(int id, ClaimsPrincipal currentUser)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) throw new ApplicationException("Not found");

            // Only admin or assigned user can delete
            var userId = GetCurrentUserId(currentUser);
            if (!IsAdmin(currentUser) && task.AssignedToUserId != userId)
                throw new UnauthorizedAccessException();

            task.IsDeleted = true;
            _repo.Update(task);
            await _repo.SaveChangesAsync();
        }
    }
}
