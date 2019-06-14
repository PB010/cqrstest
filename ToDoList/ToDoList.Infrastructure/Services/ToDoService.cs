using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Application.ToDos.Commands;
using ToDoList.Application.ToDos.Models;
using ToDoList.Persistence;

namespace ToDoList.Infrastructure.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext _context;

        public ToDoService(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToDoDto>> GetAllToDos()
        {
            var toDoFromDb =  await _context.ToDos
                .Include(t => t.ToDoPriority)
                .ToListAsync();

            return Mapper.Map<IEnumerable<ToDoDto>>(toDoFromDb);
        }

        public Task<ToDoDto> GetToDo(int id)
        {
            var toDoFromDb =  _context.ToDos.Include(t => t.ToDoPriority)
                .SingleOrDefault(t => t.Id == id);

            return Task.FromResult(ToDoDto.Create(toDoFromDb));
        }

        public Task<ToDoDto> AddNewTodo(AddNewToDoCommand command)
        {
            var newToDo = AddNewToDoCommand.Create(command);

            _context.ToDos.Add(newToDo);
            _context.SaveChanges();

            var requiredToDo = _context.ToDos
                .Include(t => t.ToDoPriority)
                .SingleOrDefault(t => t.Id == newToDo.Id);

            var dto = ToDoDto.Create(requiredToDo);

            return Task.FromResult(dto);
        }

    }
}
