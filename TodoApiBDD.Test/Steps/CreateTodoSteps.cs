using System.Net.Http.Json;
using System.Text.Json;
using TechTalk.SpecFlow;
using TodoApiBDD.Todos;

namespace TodoApiBDD.Test.Steps;

[Binding]
public class CreateTodoSteps
{
    private readonly HttpClient _client;
    private int _id;
    
    public CreateTodoSteps()
    {
        var app = new TestApp();
        _client = app.CreateDefaultClient();
    }
    
    [Given(@"API is running")]
    public async Task GivenApiIsRunning()
    {
        var response = await _client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
    }

    [When(@"a TODO with a (.*) is created with an ID")]
    public async Task WhenAtodoWithAtitleIsCreated(string title)
    {
        var todo = new Todo()
        {
            Title = title,
            IsComplete = false,
        };

        var response = await _client.PostAsJsonAsync("/todos", todo);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Todo>(json, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true})!;
        _id = result.Id;
        
        Assert.NotEqual(_id, 0);
    }

    [Then(@"the same TODO can be fetched with the ID")]
    public async Task ThenTheSameTodoIsReturnedWithAid()
    {
        var response = await _client.GetAsync($"todos/{_id}");
        
        Assert.True(response.IsSuccessStatusCode);
    }
}