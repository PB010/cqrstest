using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Application.ToDos.Commands;
using ToDoList.Application.ToDos.Models;
using ToDoList.Application.ToDos.Queries;

namespace ToDoList.Controllers
{
    [Route("/api/toDos/")]
    [ApiController]
    public class ToDosController : BaseController
    {
        public ToDosController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoDto>> GetAllToDos()
        {
            return await _mediator.Send(new GetAllToDosQuery());
        }

        [HttpGet("{id}")]
        public async Task<ToDoDto> GetToDo([FromRoute]GetSingleToDoQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ToDoDto> AddNewToDo([FromBody]AddNewToDoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
