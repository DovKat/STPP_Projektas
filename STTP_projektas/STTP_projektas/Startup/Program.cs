using System.Reflection;
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
using Swashbuckle.AspNetCore.SwaggerUI;

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


app.UseHttpsRedirection();
app.UseStaticFiles(); // This must come before routing
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        //c.RouteTemplate = "api-docs/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        c.IndexStream = () => executingAssembly.GetManifestResourceStream($"{executingAssembly.GetName().Name}.wwwroot.swagger.custom-index.html");
        c.InjectStylesheet($"./style.css");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.DocumentTitle = "Forum API V1";
        c.DefaultModelExpandDepth(2);
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(DocExpansion.List);
        c.MaxDisplayedTags(5);

        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.DefaultModelRendering(ModelRendering.Example);
        c.ShowExtensions();
    });
}
app.UseHttpsRedirection();

app.AddPostApi();
app.AddForumApi();
app.AddCommentApi();
app.Run();


