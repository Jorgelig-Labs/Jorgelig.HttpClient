# Jorgelig.HttpClient
Biblioteca para cliente rest

### Ejemplos de uso
```csharp
   try
            {
                var result = await ExecuteApi<DTOResponse>(HttpMethod.Post, loginUrl);
                
                return result;
            }
            catch (Exception e)
            {
                _log.Exception(LogEventLevel.Error, arguments: new object?[] {loginUrl});
            }
```            
