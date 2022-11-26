using TodoApiBDD.Todos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHealthChecks();
var app = builder.Build();
app.UseHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapTodos();

app.Run();

public partial class Program
{
    
}