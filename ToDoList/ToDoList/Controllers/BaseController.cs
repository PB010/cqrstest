using MediatR;

namespace ToDoList.Controllers
{
    public class BaseController
    {
        internal readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
