# Jorgelig.HttpClient
Biblioteca para cliente rest

### Ejemplos de uso
```csharp
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
```            
