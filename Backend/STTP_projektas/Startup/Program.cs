using System.Reflection;
using System.Text;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using STTP_projektas.Auth;
using STTP_projektas.Auth.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath); // Ensure the base path is correct
builder.Configuration.AddJsonFile("./startup/configs/appsettings.json", optional: false, reloadOnChange: true); // Explicitly load the config file

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
            builder => builder.WithOrigins("https://thankful-pebble-0b70dd403.4.azurestaticapps.net","http://localhost:3000") // Frontend URL
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
    })
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
    //Swagger
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
    .AddSwaggerExamplesFromAssemblyOf<ListPostDtoExample>()
    //Identity
    .AddIdentity<ForumUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<SttpDbContext>()
    .AddDefaultTokenProviders();
    //Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:ValidAudience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:ValidIssuer"];
        options.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]));
    });
    //Authorization
    builder.Services
        .AddAuthorization()
        //seeder
        .AddScoped<AuthSeeder>()
        .AddTransient<JwtTokenService>()
        .AddTransient<SessionService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
//var dbContext = scope.ServiceProvider.GetRequiredService<SttpDbContext>();
var dbSeeder = scope.ServiceProvider.GetRequiredService<AuthSeeder>();
await dbSeeder.SeedAsync();

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
app.UseCors("AllowFrontend");
app.AddPostApi();
app.AddForumApi();
app.AddAuthApi();
app.AddCommentApi();
app.UseAuthentication();
app.UseAuthorization();
app.Run();


