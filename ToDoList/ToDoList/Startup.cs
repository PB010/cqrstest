using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using ToDoList.Application.Interfaces;
using ToDoList.Application.ToDos.Commands;
using ToDoList.Application.ToDos.Models;
using ToDoList.Infrastructure.Services;
using ToDoList.Persistence;
using ToDoList.Persistence.Helper;
using ToDoList.Persistence.Models;

namespace ToDoList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddNewCommandValidator>());
            services.AddDbContext<ToDoDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionString"],
                    b => b.MigrationsAssembly(typeof(ToDoDbContext)
                        .GetTypeInfo().Assembly.GetName().Name)));
            services.AddMediatR(typeof(AddNewToDoCommand));
            services.AddScoped<IToDoService, ToDoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ToDoDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                DbSeeder.Migrate(context);
            }
            else
            {
                app.UseHsts();
            }

#pragma warning disable 618
            Mapper.Initialize(cfg =>
#pragma warning restore 618
            {
                cfg.CreateMap<ToDo, ToDoDto>().ForMember(dest => dest.ToDoPriority, opt =>
                        opt.MapFrom(src => src.ToDoPriority.Name))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                        src.Status.ToString()))
                    .ForMember(dest => dest.ToDoTime, opt => opt.MapFrom(src =>
                        src.ToDoTime.ToString("dd MMM yy HH:mm")))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>
                        src.CreatedAt.ToString("dd MMM yy HH:mm")));
                cfg.CreateMap<AddNewToDoCommand, ToDo>().ForMember(dest => dest.ToDoTime,
                        opt => opt.MapFrom(src => src.ConvertTime()))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(
                        src => DateTime.Now))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(
                        src => ToDoStatus.Open));
            });


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
