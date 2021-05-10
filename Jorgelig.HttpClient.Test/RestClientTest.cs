using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jorgelig.HttpClient.ClientHttp;
using Xunit;

namespace Jorgelig.HttpClient.Test
{
    public class RestClientTest
    {
        private static string BaseUrl => "https://jsonplaceholder.typicode.com/"; 
        private static RestClient _client;
        public RestClientTest()
        {
            _client = new RestClient(new System.Net.Http.HttpClient(), BaseUrl);
        }

        [Fact]
        public async Task RestClient_ExecuteApi_GetTitle()
        {
            var result = await _client.ExecuteApi<TodoDto>(HttpMethod.Get, "/todos/1");

            Assert.NotEmpty(result.Title);
        }

        [Fact]
        public async Task RestClient_ExecuteApi_PostTodo()
        {
            var dto = new TodoDto
            {
                Id = 1,
                Title = "Hello, World!"
            };
            var result = await _client.ExecuteApi<TodoDto>(HttpMethod.Post, "/posts",data: dto);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task RestClient_ExecuteApi_PutTodo()
        {
            var newTitle = "Hello, World2!";
            var dto = new TodoDto
            {
                Id = 1,
                Title = newTitle
            };
            var result = await _client.ExecuteApi<TodoDto>(HttpMethod.Put, "/posts/1",data: dto);
            Assert.Equal(result?.Title, newTitle);
        }
    }


    public class TodoDto
    {
        public int? UserId { get; set; }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public bool? Completed { get; set; }
    }
}
