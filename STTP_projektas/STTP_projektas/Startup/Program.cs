using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using STTP_projektas.Data;
using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using STTP_projektas.Examples;
using STTP_projektas.Factories;
using STTP_projektas.Extensions;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath); // Ensure the base path is correct
builder.Configuration.AddJsonFile("./startup/configs/appsettings.json", optional: false, reloadOnChange: true); // Explicitly load the config file

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.ExampleFilters();
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Forums API", Version = "v1" });
    })
    .AddSwaggerExamplesFromAssemblyOf<Program>()
    .AddDbContext<SttpDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddFluentValidationAutoValidation(configuration =>
    {
        configuration.OverrideDefaultResultFactoryWith<ProblemDetailsResultFactory>();
    })
    .AddSwaggerExamplesFromAssemblyOf<CommentDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<CreateCommentDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<UpdatedCommentDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<ForumDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<CreateForumDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<UpdatedForumDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<PostDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<CreatePostDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<UpdatedPostDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<ListForumDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<ListCommentDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<ListPostDtoExample>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}
app.UseHttpsRedirection();

app.AddPostApi();
app.AddForumApi();
app.AddCommentApi();
app.Run();


