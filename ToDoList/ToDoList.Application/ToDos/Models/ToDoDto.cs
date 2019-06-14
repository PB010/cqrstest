using System;
using System.Linq.Expressions;
using System.Net;
using ToDoList.Persistence.Models;

namespace ToDoList.Application.ToDos.Models
{
    public class ToDoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ToDoTime { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Status { get; set; }
        public string ToDoPriority { get; set; }
        public HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;

        public static Expression<Func<ToDo, ToDoDto>> Projection
        {
            get
            {
                return p => new ToDoDto
                {
                    Id = p.Id,
                    CreatedAt = p.CreatedAt.ToString("dd MMM yy HH:mm"),
                    Description = p.Description,
                    Name = p.Name,
                    Status = p.Status.ToString(),
                    ToDoPriority = p.ToDoPriority.Name,
                    ToDoTime = p.ToDoTime.ToString("dd MMM yy HH:mm"),
                    UpdatedAt = p.UpdatedAt.ToString()
                };
            }
        }

        public static ToDoDto Create(ToDo todo)
        {
            return Projection.Compile().Invoke(todo);
        }
    }
}
