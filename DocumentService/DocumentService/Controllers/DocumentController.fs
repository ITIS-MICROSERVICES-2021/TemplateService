namespace DocumentService

open System
open System.IO
open System.Net
open System.Net.Http
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.StaticFiles
open Microsoft.Extensions.Logging
open Microsoft.Net.Http.Headers

[<ApiController>]
[<Route("[controller]")>]
type DocumentController(logger: ILogger<DocumentController>) =
    inherit ControllerBase()

    let summaries =
        [| "Freezing"
           "Bracing"
           "Chilly"
           "Cool"
           "Mild"
           "Warm"
           "Balmy"
           "Hot"
           "Sweltering"
           "Scorching" |]

    let dir = "./data/files"

    [<HttpPost>]
    member _.Create(name: string, file: IFormFile) =
        let filePath = Path.Combine(dir, name)


        let pathExists () = Directory.Exists(dir)

        let createDir () =
            if not (pathExists ()) then
                Directory.CreateDirectory(dir) |> ignore

        let saveFile (fs: FileStream) = file.CopyTo(fs)

        createDir ()
        using (new FileStream(filePath, FileMode.Create)) saveFile

    [<HttpGet>]
    member _.Get(name: string) =
        let filePath = Path.Combine(dir, name)
        
        let provider = FileExtensionContentTypeProvider();
        
        let content = provider.TryGetContentType(filePath)
        let mutable gotContentType, contentType = content
        if not gotContentType
             then
                 contentType = "application/octet-stream" |> ignore
        
        let bytes = File.ReadAllBytes(filePath);
        FileContentResult(bytes, contentType);
