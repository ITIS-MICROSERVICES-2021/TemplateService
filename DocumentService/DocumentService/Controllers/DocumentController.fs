namespace DocumentService

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("[controller]")>]
type DocumentController(logger: ILogger<DocumentController>) =
    inherit ControllerBase()

    let service : IDocumentService = DocumentService() :> IDocumentService
    let redis : RedisService = RedisService()

    [<HttpPost>]
    member _.Create(file: IFormFile) =
        async {
            service.Create(file)
            do! redis.CreateName(file)
        }


    [<HttpGet>]
    member _.Get(name: string) =
        service.Get(name)

    [<HttpGet("GetAllFileNames")>]
    member _.GetAllFileNames() =
        service.GetAllFileNames()

    [<HttpDelete>]
    member _.Delete(name: string) =
        async {
            service.Delete(name)
            do! redis.DeleteName(name)
        }
