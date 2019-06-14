using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Application.ToDos.Commands;
using ToDoList.Application.ToDos.Models;

namespace ToDoList.Application.Interfaces
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDoDto>> GetAllToDos();
        Task<ToDoDto> GetToDo(int id);
        Task<ToDoDto> AddNewTodo(AddNewToDoCommand command);
    }
}