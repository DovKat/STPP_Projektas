using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using STTP_projektas.Auth.Model;
using STTP_projektas.Data;
using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using STTP_projektas.Examples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
namespace STTP_projektas.Extensions;

public static class Endpoints
{
    public static void AddForumApi(this WebApplication app)
    {
        var forumsGroups = app.MapGroup("/api").AddFluentValidationAutoValidation().WithTags("Forums");
        
        forumsGroups.MapGet("/forums", async (SttpDbContext dbContext) =>
        {
            return (await dbContext.Forums.ToListAsync()).Select(forum => forum.ToDto());
        })
        .WithName("GetAllForums")
        .WithMetadata(new SwaggerOperationAttribute("Get All Forums", "Returns a list of all Forums."))
        .Produces<List<ForumDto>>(StatusCodes.Status200OK);

        forumsGroups.MapGet("/forums/{forumId}", async (int forumId, SttpDbContext dbContext) =>
        {
            var forum = await dbContext.Forums.FindAsync(forumId);
            return forum == null ? Results.NotFound() : TypedResults.Ok(forum.ToDto());
        })
        .WithName("GetForumById")
        .WithMetadata(new SwaggerOperationAttribute("Get forum by ID", "Returns a forum based on the provided ID."))
        .Produces<ForumDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        forumsGroups.MapPost("/forums", [Authorize(Roles = ForumRoles.ForumUser)]async (CreateForumDto dto, SttpDbContext dbContext, HttpContext httpContext) =>
        {
            var forum = new Forum{Title = dto.Title, Description = dto.Description, CreatedAt = DateTimeOffset.UtcNow, UserId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)};
            dbContext.Forums.Add(forum);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"api/forums/{forum.Id}", forum.ToDto());
        })
        .WithName("CreateForum")
        .WithMetadata(new SwaggerOperationAttribute("Create a new forum", "Creates a new forum with the given data and returns the created post."))
        .Produces<ForumDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        forumsGroups.MapPut("/forums/{forumId}", [Authorize]async (int forumId, UpdatedForumDto dto, SttpDbContext dbContext,  HttpContext httpContext) =>
            {
                var forum = await dbContext.Forums.FindAsync(forumId);
                if (forum == null)
                {
                    return Results.NotFound();
                }
                if (!httpContext.User.IsInRole(ForumRoles.Admin) &&
                    httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != forum.UserId)
                {
                    return Results.Forbid(); //.NotFound();
                }
                forum.Description = dto.description;

                dbContext.Forums.Update(forum);
                await dbContext.SaveChangesAsync();

                return TypedResults.Ok(forum.ToDto());

            })
            .WithName("UpdateForum")
            .WithMetadata(new SwaggerOperationAttribute("Update an existing post",
                "Updates the description of an existing post with the given ID."))
            .Produces<ForumDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        forumsGroups.MapDelete("/forums/{forumId}", [Authorize]async (int forumId, SttpDbContext dbContext,  HttpContext httpContext) =>
        {
            var forum = await dbContext.Forums.FindAsync(forumId);
            if (forum == null)
            {
                return Results.NotFound();
            }
            if (!httpContext.User.IsInRole(ForumRoles.Admin) &&
                httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != forum.UserId)
            {
                return Results.Forbid(); //.NotFound();
            }
            dbContext.Forums.Remove(forum);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.NoContent();
        })
        .WithName("DeleteForum")
        .WithMetadata(new SwaggerOperationAttribute("Delete a forum", "Deletes the forum with the given ID."))
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
    
    
    public static void AddPostApi(this WebApplication app)
    {
        var postsGroups = app.MapGroup("/api/forums/{forumId}").AddFluentValidationAutoValidation().WithTags("Posts");
        
        postsGroups.MapGet("/posts", async (int forumId, SttpDbContext dbContext) =>
        { 
            var forum = await dbContext.Forums.FindAsync(forumId);
           if (forum == null)
           {
                return Results.NotFound();
           }
           return Results.Ok((await dbContext.Posts.ToListAsync())
               .Where(post => forumId == post.ForumId)
               .Select(post => post.ToDto()));
        })
        .WithName("GetAllPosts")
        .WithMetadata(new SwaggerOperationAttribute("Get All Posts", "Returns a list of all posts."))
        .Produces<List<PostDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        postsGroups.MapGet("/posts/{postId}", async (int forumId, int postId, SttpDbContext dbContext) =>
        {
            var post = await dbContext.Posts.FindAsync(postId);
            return post == null || post.ForumId != forumId ? Results.NotFound() : TypedResults.Ok(post.ToDto());
        })
        .WithName("GetPostById")
        .WithMetadata(new SwaggerOperationAttribute("Get Post by ID", "Returns a post based on the provided ID."))
        .Produces<PostDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        postsGroups.MapPost("/posts", [Authorize(Roles = ForumRoles.ForumUser)]async (int forumId, CreatePostDto dto, SttpDbContext dbContext, HttpContext httpContext) => 
            { 
                var forum = await dbContext.Forums.FindAsync(forumId);
                if (forum == null)
                {
                    return Results.NotFound();
                }
                var post = new Post{Title = dto.Title, ForumId = forumId, Description = dto.Description, CreatedAt = DateTimeOffset.UtcNow, UserId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) };
                dbContext.Posts.Add(post);

                await dbContext.SaveChangesAsync(); 
                return TypedResults.Created($"api/forums/{forumId}/posts/{post.Id}", post.ToDto());
            })
        .WithName("CreatePost")
        .WithMetadata(new SwaggerOperationAttribute("Create a new post", "Creates a new post with the given data and returns the created post."))
        .Produces<PostDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        postsGroups.MapPut("/posts/{postId}",  [Authorize]async (int forumId, UpdatedPostDto dto, int postId, SttpDbContext dbContext, HttpContext httpContext) =>
        {
            var post = await dbContext.Posts.FindAsync(postId);
            if (post == null || post.ForumId != forumId )
            {
                return Results.NotFound();
            }

            if (!httpContext.User.IsInRole(ForumRoles.Admin) &&
                httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != post.UserId)
            {
                return Results.Forbid(); //.NotFound();
            }

            post.Description = dto.description;

            dbContext.Posts.Update(post);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.Ok(post.ToDto());

        })
        .WithName("UpdatePost")
        .WithMetadata(new SwaggerOperationAttribute("Update an existing post", "Updates the description of an existing post with the given ID."))
        .Produces<PostDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        postsGroups.MapDelete("/posts/{postId}", [Authorize]async (int forumId, int postId, SttpDbContext dbContext, HttpContext httpContext) =>
        {
            var post = await dbContext.Posts.FindAsync(postId);
            if (post == null || post.ForumId != forumId)
            {
                return Results.NotFound();
            }
            if (!httpContext.User.IsInRole(ForumRoles.Admin) &&
                httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != post.UserId)
            {
                return Results.Forbid(); //.NotFound();
            }
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.NoContent();
        })
        .WithName("DeletePost")
        .WithMetadata(new SwaggerOperationAttribute("Delete a post", "Deletes the post with the given ID."))
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }

    public static void AddCommentApi(this WebApplication app)
    {
        var commentsGroups = app.MapGroup("/api/forums/{forumId}/posts/{postId}").AddFluentValidationAutoValidation()
            .WithTags("Comments");

        commentsGroups.MapGet("/comments", async (int forumId, int postId, SttpDbContext dbContext) =>
        {
            var forum = await dbContext.Forums.FindAsync(forumId);
            var post = await dbContext.Posts.FindAsync(postId);
            if (forum == null|| post == null)
            {
                return Results.NotFound();
            }
            return Results.Ok((await dbContext.Comments.ToListAsync())
                .Where(comment =>  postId == comment.PostId)
                .Select(comment => comment.ToDto()));
        })
        .WithName("GetAllcomments")
        .WithMetadata(new SwaggerOperationAttribute("Get All comment", "Returns a list of all comments."))
        .Produces<List<CommentDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        commentsGroups.MapGet("/comments/{commentId}", async (int commentId, int forumId, int postId, SttpDbContext dbContext) =>
        {
            var comment = await dbContext.Comments.FindAsync(commentId);
            return comment == null || comment.PostId != postId ? Results.NotFound() : TypedResults.Ok(comment.ToDto());
        })
        .WithName("GetCommentById")
        .WithMetadata(new SwaggerOperationAttribute("Get comment by ID", "Returns a comment based on the provided ID."))
        .Produces<CommentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        commentsGroups.MapPost("/comments/", [Authorize(Roles = ForumRoles.ForumUser)]async (int postId, int forumId, CreateCommentDto dto, SttpDbContext dbContext, HttpContext httpContext) => 
            { 
                var post = await dbContext.Posts.FindAsync(postId);
                if (post == null)
                {
                    return Results.NotFound();
                }
            var comment = new Comment{ PostId = postId, Description = dto.Description, CreatedAt = DateTimeOffset.UtcNow, UserId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)};
            dbContext.Comments.Add(comment);

            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"api/forums/{forumId}/posts/{postId}/comments/{comment.Id}", comment.ToDto());
        })
        .WithName("CreateComment")
        .WithMetadata(new SwaggerOperationAttribute("Create a new comment", "Creates a new comment with the given data and returns the created comment."))
        .Produces<CommentDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        commentsGroups.MapPut("/comments/{commentId}", [Authorize]async (int commentId, int forumId, UpdatedCommentDto dto, int postId, SttpDbContext dbContext, HttpContext httpContext) =>
        {
            var comment = await dbContext.Comments.FindAsync(commentId);
            if (comment == null || comment.PostId != postId )
            {
                return Results.NotFound();
            }
            if (!httpContext.User.IsInRole(ForumRoles.Admin) &&
                httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != comment.UserId)
            {
                return Results.Forbid(); //.NotFound();
            }
            comment.Description = dto.description;

            dbContext.Comments.Update(comment);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.Ok(comment.ToDto());

        })
        .Accepts<UpdatedCommentDto>("application/json")
        .WithName("UpdateComment")
        .WithMetadata(new SwaggerOperationAttribute("Update an existing comment", "Updates the description of an existing comment with the given ID."))
        .Produces<CommentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status422UnprocessableEntity)
        ;

        commentsGroups.MapDelete("/comments/{commentId}", [Authorize]async (int commentId, int forumId, int postId, SttpDbContext dbContext, HttpContext httpContext) =>
        {
            var comment = await dbContext.Comments.FindAsync(commentId);
            if (comment == null || comment.PostId != postId)
            {
                return Results.NotFound();
            }
            if (!httpContext.User.IsInRole(ForumRoles.Admin) &&
                httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != comment.UserId)
            {
                return Results.Forbid(); //.NotFound();
            }
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.NoContent();
        })
        .WithName("DeleteComment")
        .WithMetadata(new SwaggerOperationAttribute("Delete a comment", "Deletes the comment with the given ID."))
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}