using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Application.ToDos.Models;

namespace ToDoList.Application.ToDos.Queries
{
    public class GetAllToDosQuery : IRequest<IEnumerable<ToDoDto>>
    {
    }

    public class GetAllToDosHandler : IRequestHandler<GetAllToDosQuery, IEnumerable<ToDoDto>>
    {
        private readonly IToDoService _service;

        public GetAllToDosHandler(IToDoService service)
        {
            _service = service;
        }
        public async Task<IEnumerable<ToDoDto>> Handle(GetAllToDosQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllToDos();
        }
    }
}
