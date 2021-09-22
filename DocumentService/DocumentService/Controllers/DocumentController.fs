namespace DocumentService

open System
open System.IO
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

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
        
        let saveFile (fs : FileStream) =
            file.CopyTo(fs);

        createDir ()
        using (new FileStream(filePath, FileMode.Create)) saveFile