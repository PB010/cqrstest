using FluentValidation;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Application.ToDos.Models;
using ToDoList.Persistence.Models;

namespace ToDoList.Application.ToDos.Commands
{
    public class AddNewToDoCommand : IRequest<ToDoDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public int ToDoPriorityId { get; set; }

        public DateTime ConvertTime()
        {
            return DateTime.Parse(Time);
        }

        public static Expression<Func<AddNewToDoCommand, ToDo>> Projection
        {
            get
            {
                return p => new ToDo
                {
                    Name = p.Name,
                    CreatedAt = DateTime.Now,
                    Description = p.Description,
                    Status = ToDoStatus.Open,
                    ToDoPrioritiesId = p.ToDoPriorityId,
                    ToDoTime = p.ConvertTime(),
                };
            }
        }

        public static ToDo Create(AddNewToDoCommand command)
        {
            return Projection.Compile().Invoke(command);
        }
    }

    public class AddNewCommandHandler : IRequestHandler<AddNewToDoCommand, ToDoDto>
    {
        private readonly IToDoService _service;

        public AddNewCommandHandler(IToDoService service)
        {
            _service = service;
        }

        public Task<ToDoDto> Handle(AddNewToDoCommand request, CancellationToken cancellationToken)
        {
            var toDoForDb =  _service.AddNewTodo(request);
            
            return toDoForDb;
        }
    }

    public class AddNewCommandValidator : AbstractValidator<AddNewToDoCommand>
    {
        public AddNewCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.ToDoPriorityId).NotEmpty().WithMessage("Priority is required");
            RuleFor(x => x.Time).NotEmpty().WithMessage("Time is required");
        }
    }
}
