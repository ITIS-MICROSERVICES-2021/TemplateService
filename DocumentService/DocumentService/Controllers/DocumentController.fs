namespace DocumentService

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("[controller]")>]
type DocumentController(logger: ILogger<DocumentController>) =
    inherit ControllerBase()
    
    let service : IDocumentService = new DocumentService() :> IDocumentService

    [<HttpPost>]
    member _.Create(file: IFormFile) =
        service.Create(file)

    [<HttpGet>]
    member _.Get(name: string) =
        service.Get(name)
    
    [<HttpGet("GetAllFileNames")>]
    member _.GetAllFileNames() =
        service.GetAllFileNames()

    [<HttpDelete>]
    member _.Delete(name: string) =
        service.Delete(name)