using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Application.ToDos.Models;
using ToDoList.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Application.ToDos.Queries
{

    public class GetSingleToDoQuery : IRequest<ToDoDto>
    {
        public int Id { get; set; }
        
    }

    public class GetSingleQueryHandler : IRequestHandler<GetSingleToDoQuery, ToDoDto>
    {
        private readonly IToDoService _service;
        
        public GetSingleQueryHandler(IToDoService service)
        {
            _service = service;
        }
        public Task<ToDoDto> Handle(GetSingleToDoQuery request, CancellationToken cancellationToken)
        {
           
            var toDoToReturn = _service.GetToDo(request.Id);
            return toDoToReturn;
        }
    }

    public class GetSingleQueryValidator : AbstractValidator<GetSingleToDoQuery>
    {
        public GetSingleQueryValidator(ToDoDbContext context)
        {
            RuleFor(x => x.Id).Must(x => context.ToDos.Any(c => c.Id == x)).WithErrorCode("ERR-401")
                .WithMessage("There is no such todo in the system.");
        }

    }

    public static class HelperMethod
    {
        public static StatusCodeResult Help()
        {
            return new StatusCodeResult(401);
        }
    }
}
