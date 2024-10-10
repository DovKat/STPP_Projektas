using DbContext = STTP_projektas.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<DbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var topicsGroups = app.MapGroup("/api");

topicsGroups.MapGet("/topics", () => { });
topicsGroups.MapGet("/topics/{topicId}", (int topicId) => { });
topicsGroups.MapPost("/topics", (CreateTopicDto dto) => "POST");
topicsGroups.MapPut("/topics/{topicId}", (UpdatedTopicDto dto, int topicId) => "PUT");
topicsGroups.MapDelete("/topics/{topicId}", () => "DELETE");


app.Run();

public record TopicDto(int Id, string Title, string Description, DateTimeOffset CreatedOn);

public record CreateTopicDto(string Title, string Description);
public record UpdatedTopicDto(string description);