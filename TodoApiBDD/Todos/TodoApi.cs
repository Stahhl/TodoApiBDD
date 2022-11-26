namespace TodoApiBDD.Todos;

internal static class TodoApi
{
    private static readonly Dictionary<int, Todo?> Todos = new();

    public static RouteGroupBuilder MapTodos(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/todos");

        group.WithTags("Todos");

        group.MapGet("/", () => Results.Ok(Todos.Select(x => x.Value)));

        group.MapGet("/{id}", (int id) =>
            {
                var todo = Todos[id];

                return todo is not null ? Results.Ok(todo) : Results.NotFound(id);
            })
            .Produces<Todo?>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", (Todo todo) =>
            {
                var id = Todos.Count + 1;
                todo.Id = id;
                Todos[id] = todo;

                return Results.Created($"/todos/{todo.Id}", todo);
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        // group.MapPut("/{id}", async (TodoDbContext db, int id, TodoItem todo, CurrentUser owner) =>
        // {
        //     if (id != todo.Id)
        //     {
        //         return Results.BadRequest();
        //     }
        //
        //     var rowsAffected = await db.Todos.Where(t => t.Id == id && (t.OwnerId == owner.Id || owner.IsAdmin))
        //                                      .ExecuteUpdateAsync(updates => 
        //                                         updates.SetProperty(t => t.IsComplete, todo.IsComplete)
        //                                                .SetProperty(t => t.Title, todo.Title));
        //
        //     if (rowsAffected == 0)
        //     {
        //         return Results.NotFound();
        //     }
        //
        //     return Results.Ok();
        // })
        // .Produces(Status400BadRequest)
        // .Produces(Status404NotFound)
        // .Produces(Status200OK);

        group.MapDelete("/{id}", (int id) =>
            {
                Todos[id] = null;
                return Results.Ok();
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK);

        return group;
    }
}